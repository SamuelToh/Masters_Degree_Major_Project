using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BioPatMLEditor.ResultsPanel.DisplayEngine
{
    /// <summary>
    /// This class comprises of all the complex algorithm used for rendering
    /// our text display in the Rich Text Box of Result panel.
    /// </summary>
    public static  class ResultDisplayLogic
    {
        /// <summary>
        /// Breakline to accomodate different platform.
        /// </summary>
        static string NEWLINE = System.Environment.NewLine;

        /// <summary>
        /// This setting determines the amount of character to render prior to 
        /// start displaying the hit characters.
        /// </summary>
        static int CHARACTERS_BEFORE_HITSTART = 150;

        /// <summary>
        /// Determines the amount of character to be displayed on each line
        /// </summary>
        static int CHARACTERS_PER_LINE = 30;

        /// <summary>
        /// This setting determines the amount of character to render after the last
        /// character of hit.
        /// </summary>
        static int CHARACTERS_AFTER_HITEND = 150;

        /// <summary>
        /// This method returns a simple paragraph text of header. 
        /// </summary>
        /// <param name="patternName">The name of pattern we want to display</param>
        /// <param name="hitStart">Start position of our hit</param>
        /// <param name="hitEnd">End position of our hit</param>
        /// <returns>A paragraph of text for our header.</returns>
        static public Paragraph GetRichHeader
                            (String patternName, int hitStart, int hitEnd)
        {
            // Create our bold header bold text.
            Bold myBold = new Bold();
            myBold.Inlines.Add("Sequence Hits Viewer");

            Run myRun2 = new Run();
            myRun2.Text = "================";
            myRun2.Text += NEWLINE;
            myRun2.Text += NEWLINE;
            myRun2.Text += "Matched Start Position : [ " + hitStart + " ]";
            myRun2.Text += NEWLINE;
            myRun2.Text += "Matched End   Position : [ " + hitEnd + " ]";
            myRun2.Text += NEWLINE;
            myRun2.Text += NEWLINE;
            myRun2.Text += "================ " + patternName + " ================";
            myRun2.Text += NEWLINE;

            Paragraph Header = new Paragraph();

            Header.Inlines.Add(myBold);
            Header.Inlines.Add(myRun2);

            return Header;
        }

        /// <summary>
        /// Similar to the method above however this implementation is slightly more
        /// complicated. This method compiles a paragraph containing information about
        /// the sequence's start and end position.
        /// 
        /// PreCondition: Have to ensure the end position is no longer than the sequence length.
        /// </summary>
        /// <param name="letters">the sequence of characters to render</param>
        /// <param name="hitStart">Start position of the hit</param>
        /// <param name="hitEnd">End position of hit</param>
        /// <returns></returns>
        static public Paragraph GetHitDetails(string letters, int hitStart, int hitEnd)
        {
            #region Checks the pre-condition

            if (hitEnd > letters.Length)
                throw new Exception("The end position shouldn't be longer than the sequence");

            #endregion

            #region Convert the bio start position of 1 to computer language's 0

            hitStart -= 1;
            hitEnd   -= 1;

            #endregion

            Paragraph resultPara = new Paragraph();
            int displayStartPos = CalcStartPosition(hitStart);
            int lineBreakRule = displayStartPos ;

            //for (int i = 0; i < letters.Length; i++)
            for (int i = displayStartPos; i < CalcEndPosition(hitEnd, letters); i++)
            {
                Run lineDesc = new Run();
                lineDesc.FontSize = 10;
                lineDesc.FontFamily = new FontFamily("Verdana");

                #region Rules for appending of 3 blank spaces for every set of 10 characters

                if (i % 10 == 0)
                    lineDesc.Text += ("   ");

                #endregion

                #region Rules for a new line break

                //if (i % 30 == 0)
                if (i == 0) //first line definitely a new line
                {
                    lineDesc.Text += (NEWLINE
                                       + "Line " + CompileNumber((i + 1), letters) +
                                       " ... " +
                                       CompileNumber((i + 1 + 30), letters) +
                                       "\t\t");
                    resultPara.Inlines.Add(lineDesc); //Add the lines string into paragraph first
                    lineDesc = new Run(); //renew out run
                    lineBreakRule += CHARACTERS_PER_LINE; //set the next break
                }
                else if (i % lineBreakRule == 0)
                {
                    lineDesc.Text += (NEWLINE
                                        + "Line " + CompileNumber((i + 1), letters) +
                                        " ... " +
                                        CompileNumber((i + 1 + 30), letters) +
                                        "\t\t");
                    resultPara.Inlines.Add(lineDesc); //Add the lines string into paragraph first
                    lineDesc = new Run(); //renew out run
                    lineBreakRule += CHARACTERS_PER_LINE; //set the next break
                }
                #endregion

                #region if (index is within hit zone)

                if (i >= hitStart
                            && i <= hitEnd)
                {
                    lineDesc.FontSize = 19; //magnify hits
                    lineDesc.FontWeight = FontWeights.Bold;
                    lineDesc.Foreground = new SolidColorBrush(Colors.Black);
                }

                #endregion
                #region else we set the attribute of run to be normal alphabet coloring
                
                else
                {
                    lineDesc.FontSize = 14;
                    lineDesc.FontFamily = new FontFamily("Verdana");
                    SetRunAttr(lineDesc, letters[i]);
                }

                #endregion

                lineDesc.Text += (letters[i].ToString() + " "); // otherwise append the character
                resultPara.Inlines.Add(lineDesc);
            }

            return resultPara;
        }

        /// <summary>
        /// This implementation sets the color text of different characters of Alphabet
        /// </summary>
        /// <param name="run"></param>
        /// <param name="character"></param>
        static private void SetRunAttr(Run run, char character)
        {
            
            switch (character.ToString().ToUpper())
            {
                #region "DNA" Alphabets

                case "T":
                    { run.Foreground = new SolidColorBrush(Colors.Red); return; }

                case "C":
                    { run.Foreground = new SolidColorBrush(Colors.Blue); return; }

                case "A":
                    { run.Foreground = new SolidColorBrush(Colors.Green); return; }

                case "G":
                    { run.Foreground = new SolidColorBrush(Colors.Magenta); return; }

                #endregion

                #region "RNA" Alphabets

                //Alphabet G already defined in DNA section

                case "U":
                    { run.Foreground = new SolidColorBrush(Colors.Orange); return; }

                #endregion

                #region "Amino Acid" alphabets

                case "R":
                    { run.Foreground = new SolidColorBrush(Colors.Purple); return; }

                case "N":
                    { run.Foreground = new SolidColorBrush(Colors.Yellow); return; }

                case "D":
                    { run.Foreground = new SolidColorBrush(Colors.Brown); return; }

                case "Q":
                    { run.Foreground = new SolidColorBrush(Colors.DarkGray); return; }

                case "E":
                    { run.Foreground = new SolidColorBrush(Colors.Cyan); return; }

                case "H":
                    { run.Foreground = new SolidColorBrush(Colors.Gray); return; }

                case "I":
                    { run.Foreground = new SolidColorBrush(Colors.Green); return; }

                case "L":
                    { run.Foreground = new SolidColorBrush(Colors.Magenta); return; }

                case "K":
                    { run.Foreground = new SolidColorBrush(Colors.Yellow); return; }

                case "M":
                    { run.Foreground = new SolidColorBrush(Colors.Orange); return; }

                case "F":
                    { run.Foreground = new SolidColorBrush(Colors.LightGray); return; }
                    
                case "P":
                    { run.Foreground = new SolidColorBrush(Colors.Red); return; }

                case "S":
                    { run.Foreground = new SolidColorBrush(Colors.Purple); return; }

                case "W":
                    { run.Foreground = new SolidColorBrush(Colors.Blue); return; }

                #endregion
            }
        }

        static private String CompileNumber(int hitPos, string characters)
        {
            //We want to extract the total number of chracters in numeric form
            int strLength = characters.Length.ToString().Length;

            return hitPos.ToString().PadLeft(strLength, '0');

        }

        /// <summary>
        /// Calculates the index for rendering the first character.
        /// </summary>
        /// <param name="hitStart">start position of the hit</param>
        /// <returns></returns>
        static private int CalcStartPosition(int hitStart)
        {
            return hitStart - CHARACTERS_BEFORE_HITSTART < 0 ? 0 
                :  RoundDownInt(hitStart);
        }

        static int RoundDownInt(int integer)
        {
            double rounded = (integer - CHARACTERS_BEFORE_HITSTART) / 10;
            return Convert.ToInt16(rounded * 10);
        }
        
        /// <summary>
        /// Calculates the index for rendering the last character.
        /// </summary>
        /// <param name="hitEnd">index position of the last character in hit</param>
        /// <param name="characters">the sequence</param>
        /// <returns></returns>
        static private int CalcEndPosition(int hitEnd, string characters)
        {
            //We want to extract the total number of chracters in numeric form
            int strLength = characters.Length;

            return hitEnd + CHARACTERS_AFTER_HITEND > strLength ? strLength
                 : hitEnd + CHARACTERS_AFTER_HITEND;

        }
    }
}
