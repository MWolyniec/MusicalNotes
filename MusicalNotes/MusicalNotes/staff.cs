
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.IO;
using System.Reflection;

namespace MusicalNotes
{
    /// <summary>
    /// Od 0 (dolna podlinia) do 12 (górna nadlinia)
    /// </summary>
    public enum StaffLine
    {

        Underunderfirst, UnderFirst, First, UnderSecond, Second, UnderThird, Third, UnderFourth, Fourth,
        UnderFifth, Fifth, Undersixth, Sixth
    }
    /// <summary>
    /// długości nut i pauz przełożone na proporcjonalne wartości liczbowe
    /// </summary>
    public enum NoteLength
    {
        Sixty_FourthRest = -64, HundredTwenty_EighthRest = -128, TwoHundredFifty_SixthRest = -256,
        EighthRest = -8, SixteenthRest = -16, Thirty_SecondRest = -32,
        WholeRest = -1, HalfRest = -2, QuarterRest = -4,
        Empty = 0,
        WholeNote = 1, HalfNote = 2, QuarterNote = 4,
        EighthNote = 8, SixteenthNote = 16, Thirty_SecondNote = 32,
        Sixty_FourthNote = 64, HundredTwenty_EighthNote = 128, TwoHundredFifty_SixthNote = 256
    }



    public class StaffDraw
    {
        /// <summary>
        /// Potrzeba przekopiować z danego fonta poszczególne znaki i dokończyć słownik
        /// </summary>
        public readonly Dictionary<NoteLength, string> noteDicc = new Dictionary<NoteLength, string>
        {
            {NoteLength.HalfNote, "W"},
            {NoteLength.QuarterNote, "Q"},
            {NoteLength.EighthNote, "E"}

        };
        

        float noteInterval;

        SKTypeface noteTypeface;

        /// <summary>
        /// Inicjalizuję fonty i obiekt do wyciągania rizorsów
        /// </summary>
        internal StaffDraw()
        {
            ResourceHandler resourceHandler = new ResourceHandler();
            Stream stream = resourceHandler.GetAResourceStreamByResourceName("noted.ttf"); //nazwę fonta zmień odpowiednio

            noteTypeface = SKTypeface.FromStream(stream);//SKTypeface.FromFamilyName("Arial"); //Arial testowo
        }

        internal void SkyDraw(SKPaintSurfaceEventArgs args, List<KeyValuePair<NoteLength, StaffLine>> script = null)
        {
            Dictionary<StaffLine, float> linesVerticalPositions = new Dictionary<StaffLine, float>();
            SKImageInfo screenSize = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.Black);
            int marginX = 5;
            float lineThicc;
            float spaceBetweenLines;

            float scale = (float)screenSize.Height/(float)screenSize.Width ; //potrzebuje casta, nie wierz kompilatorowi, nie usuwaj.

            // ewentualna korekta skali
            //scale = (float)(scale * 0.8);

            noteInterval = (float)screenSize.Width / 20;
            spaceBetweenLines = (float)(screenSize.Height / 32);
            lineThicc = (float)(screenSize.Height / 300);
            float noteStart = noteInterval * (float)1.5; // przerwa z lewej strony jak duża w porównaniu do przerw między nutami

            if (screenSize.Height > screenSize.Width) 
            {
                spaceBetweenLines = spaceBetweenLines / scale;
                lineThicc = lineThicc / scale;
                noteInterval = noteInterval * scale;
                noteStart = noteStart / scale;
            }

            SKPaint linePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.DeepPink,
                StrokeWidth = lineThicc
            };
            

            for (int line = 1; line < 6; line++)
            {
                int letsDrawFromTheBottom = 6 - line;
                var positionY = 0 + ((spaceBetweenLines + lineThicc / 2) * letsDrawFromTheBottom) + (spaceBetweenLines * 3);

                float x0 = 0 + marginX;
                float y0 = positionY;
                float x1 = (float)screenSize.Width - marginX;
                float y1 = positionY;
                canvas.DrawLine(x0, y0, x1, y1, linePaint);


                if (line == 1) 
                {
                    float underUnderLineY = positionY + (spaceBetweenLines + lineThicc);
                    linesVerticalPositions.Add((StaffLine)0, underUnderLineY);
                }
                float underLineY = positionY + ((spaceBetweenLines + lineThicc / 2) / 2); 
                linesVerticalPositions.Add((StaffLine)(line * 2 - 1), underLineY);
                linesVerticalPositions.Add((StaffLine)(line * 2), positionY);

                if (line == 5) 
                {
                    linesVerticalPositions.Add((StaffLine)(line * 2 + 1), positionY - ((spaceBetweenLines + lineThicc / 2) / 2));
                    linesVerticalPositions.Add((StaffLine)(line * 2 + 2), positionY - (spaceBetweenLines + lineThicc / 2));
                }

            }
            


            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.AntiqueWhite,

                TextSize = noteInterval,// scale, //ewentualne doskalowanie fonta
                TextAlign = SKTextAlign.Left,
                Typeface = noteTypeface
            };
            // Display text //testowe wyświetlanie pozycji itp
            //  var assembly = new AssemblyName(typeof(App).GetTypeInfo().Assembly.FullName); //Assembly.GetExecutingAssembly().FullName;//.ToString();//GetName().Name;
            // float keyPositionX = noteInterval;
            // float keyPositionY = linesVerticalPositions[StaffLine.Third] + textPaint.TextSize / 10;
            // canvas.DrawText("X: "+keyPositionX.ToString(), keyPositionX+50, keyPositionY, textPaint); 
            //canvas.DrawText(assembly.Name, keyPositionX+50, keyPositionY+200, textPaint);
            // textPaint.TextSize = noteInterval;// scale

           


            noteInterval = noteInterval - textPaint.TextSize / 10; //środek nut nie jest w samych nutach, a np na patyczku nóżce czy no
            if (script != null)
            {
                int i = 1;
                foreach (KeyValuePair<NoteLength, StaffLine> noteOnLine in script)
                {
                    string drawedNote = noteDicc[noteOnLine.Key];
                    float notePositionY = linesVerticalPositions[noteOnLine.Value] + textPaint.TextSize / 10;
                    float notePositionX = noteStart + noteInterval * i;
                    canvas.DrawText(noteDicc[noteOnLine.Key], notePositionX, notePositionY, textPaint);
                    i++;
                }
            }

            
        }



    }
}
