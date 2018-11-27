using SkiaSharp.Views.Forms;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MusicalNotes
{
    public partial class MainPage : ContentPage
    {

        public KeyValuePair<NoteLength, StaffLine> noteWithItsLine;
        public List<KeyValuePair<NoteLength, StaffLine>> sampleList;


        public StaffDraw staff;

        public MainPage()
        {
            InitializeComponent();

            //próbka 

            staff = new StaffDraw();
            sampleList = new List<KeyValuePair<NoteLength, StaffLine>>();
            for(int i = 2; i < 11; i++)
            {

                if(i < 5) noteWithItsLine = new KeyValuePair<NoteLength, StaffLine>(NoteLength.HalfNote, (StaffLine)i);
                else if((i > 4) && (i < 8)) noteWithItsLine = new KeyValuePair<NoteLength, StaffLine>(NoteLength.QuarterNote, (StaffLine)i);
                else noteWithItsLine = new KeyValuePair<NoteLength, StaffLine>(NoteLength.EighthNote, (StaffLine)i);
                sampleList.Add(noteWithItsLine);
            };
        }
        public void OnDżordż(object sender, SKPaintSurfaceEventArgs args)
        {
            staff.SkyDraw(args, sampleList);
        }
      
    }
}
