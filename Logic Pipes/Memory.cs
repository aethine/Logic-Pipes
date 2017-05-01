using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Ciloci.Flee; //thanks flee!! (http://flee.codeplex.com/)

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
        public static string ConvertSentence(string sentence)
        {
            string[] w = sentence.Split(' ');
            string tor = null;
            foreach (string x in w)
                if (x.StartsWith("$")) tor += GetVar(x.Split('$')[1] + " ");
                else tor += x + " ";
            return tor.Trim();
        }
        public static int EquISentence(string sentence)
        {
            return (int) new DataTable().Compute(ConvertSentence(sentence), "");
        }
        public static bool EquBSentence(string sentence)
        {
            string[] exps = ConvertSentence(sentence).Split(' ');
            string fsentence = null;
            for (int x = 0; x < exps.Length; x++)
            {
                if (exps[x] == "0") fsentence += "false ";
                else if (exps[x] == "1") fsentence += "true ";
                else fsentence += exps[x] + " ";
            }
            ExpressionContext context = new ExpressionContext();
            IGenericExpression<bool> e = context.CompileGeneric<bool>(fsentence);
            return e.Evaluate();
        }

    }
}
