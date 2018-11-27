using System;
using System.IO;
using System.Reflection;

namespace MusicalNotes
{
    class ResourceHandler
    {

        public Stream GetAResourceStreamByResourceName(string fullResourceName)
        {


            var assembly = typeof(App).GetTypeInfo().Assembly;


            var resourceManifest = this.GetType().Assembly.GetManifestResourceNames();
            string resourcePath = null;
            foreach (string resource in resourceManifest)
            {
                if (resource.Contains(fullResourceName)) resourcePath = resource;
            }
            if (resourcePath == null) throw new Exception("Nie znalazło ścieżki do zasobu");


            var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                return null;
            }
            return stream;
        }
    }
}
