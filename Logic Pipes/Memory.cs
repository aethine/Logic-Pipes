using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic_Pipes
{
    static class Memory
    {
        private class Var
        {
            public string name, value;
            public Var(string name, string value) { this.name = name; this.value = value; }
        }
        static List<Var> Vars = new List<Var>();
        static Var FindVar(string name)
        {
            foreach (Var v in Vars) if (v.name == name) return v;
            return null;
        }
        static bool VarExists(string name)
        {
            return FindVar(name) != null;
        }
        static bool VarExists(Var v)
        {
            return VarExists(v.name);
        }
        public static void NewVar(string name, string value) { if(!VarExists(name)) Vars.Add(new Var(name, value)); }
        public static void DelVar(string name) { Vars.Remove(FindVar(name)); }
        public static void SetVar(string name, string newvalue)
        {
            if (VarExists(name))
            {
                foreach (Var v in Vars)
                    if (v.name == name) v.value = newvalue;
            }
        }
        public static string GetVar(string name)
        {
            if (VarExists(name))
                return FindVar(name).value;
            else return null;
        }
    }
}
