using System.Collections.Generic;
namespace System
{
    class FileList
    {
        private class element
        {
            public string value;
        }
        string path, comment;
        element[] listset;
        public FileList(string Path, char Comment = ';')
        {
            if (!IO.File.Exists(Path)) { throw new IO.FileNotFoundException(); }
            path = Path;comment = Comment.ToString(); reload();
        }
        public void reload()
        {
            string[] templines = IO.File.ReadAllLines(path);
            { //removing coments
                string[] t;
                int c = 0;
                foreach (string s in templines) if (!(s.StartsWith(comment) || string.IsNullOrWhiteSpace(s))) c++;
                t = new string[c];
                int cc = 0;
                foreach (string s in templines) if (!(s.StartsWith(comment) || string.IsNullOrWhiteSpace(s))) { t[cc] = s; cc++; }
                templines = t;
            }
            listset = new element[templines.Length];
            for (int x = 0; x != listset.Length; x++)
            {
                listset[x] = new element();
                listset[x].value = templines[x];
            }
        }
        public int elementCount()
        {
            int g = 0;
            foreach (element o in listset) g++;
            return g;
        }
        public void newElement(string val)
        {
            string[] templines = IO.File.ReadAllLines(path);
            List<string> temp = new List<string>();
            foreach (string t in templines) { temp.Add(t); }
            temp.Add(null);
            string[] f = temp.ToArray();
            f[templines.Length] = val;
            templines = f;
            IO.File.WriteAllLines(path, templines);
        }
        public void delElement(string val)
        {
            bool l = true;
            string[] templines = IO.File.ReadAllLines(path);
            List<string> temp = new List<string>();
            foreach (string t in templines) { if (t != val) temp.Add(t); else l = false; }
            if (l) throw new ArgumentException("Could not find element");
            templines = temp.ToArray();
            IO.File.WriteAllLines(path, templines);
        }
        public string[] getAllElements()
        {
            List<string> v = new List<string>();
            foreach (element o in listset) v.Add(o.value);
            return v.ToArray();
        }
        public string getNameFromId(int id)
        {
            for (int x = 0; x != id + 1; x++)
                try { if (x == id) return listset[x].value; } catch (IndexOutOfRangeException) { }
            throw new ArgumentException("Could not find element");
        }
    }
}
