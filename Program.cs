using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisharmonyHookDetector
{
    public static class Program
    {
        static void Main()
        {
            /*
             * Написано на коленке за 10 минут
             * Author: https://github.com/naweka
             */

            Harmony h = new Harmony("a");
            RuntimeHelpers.PrepareMethod(typeof(Program).GetMethod("aaa").MethodHandle);
            
            
            Console.WriteLine("PREFIX before hook: " + IsHookedByHarmony(typeof(Program).GetMethod("aaa")));
            
            // PREFIX
            h.Patch(
                typeof(Program).GetMethod("aaa"), 
                new HarmonyMethod(typeof(Program).GetMethod("bbb")));


            Console.WriteLine("PREFIX after hook: " + IsHookedByHarmony(typeof(Program).GetMethod("aaa")));
            h.UnpatchAll("a");

            Console.WriteLine("PREFIX after unpatch (LOL): " + IsHookedByHarmony(typeof(Program).GetMethod("aaa")));

            Console.WriteLine("POSTFIX before hook: " + IsHookedByHarmony(typeof(Program).GetMethod("aaa2")));
            
            // POSTFIX
            h.Patch(
                    typeof(Program).GetMethod("aaa2"),
                    null,
                    new HarmonyMethod(typeof(Program).GetMethod("bbb")));

            Console.WriteLine("POSTFIX after hook: " + IsHookedByHarmony(typeof(Program).GetMethod("aaa2")));
            h.UnpatchAll("a");
            Console.WriteLine("POSTFIX after unpatch (LOL): " + IsHookedByHarmony(typeof(Program).GetMethod("aaa2")));

            Console.ReadLine();
        }

        public static void aaa() 
        {
            Console.WriteLine("12312312" +  int.Parse("8464646"));
        }

        public static void aaa2()
        {
            Console.WriteLine("12312312" + int.Parse("8464646"));
        }

        public static void bbb()
        {
            Console.WriteLine("hi");
        }


        public static unsafe bool IsHookedByHarmony(MethodBase method)
        {
            // Dear Cabbo, when you set hook, please, dont
            // broke the whole app
            //
            // For example this should give us E9, not zero:
            // var cabboAss = stackalloc byte[1];
            // cabboAss[0] = 0xE9;
            // Console.WriteLine(Marshal.ReadByte(new IntPtr(cabboAss)));

            var ptr = method.MethodHandle.GetFunctionPointer();
            var first = *(byte*)(void*)ptr;
            //Console.WriteLine(first.ToString("X"));

            return first == 0xE9;
        }
    }
}
