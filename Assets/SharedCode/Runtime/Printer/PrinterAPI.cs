#if UNITY_EDITOR || UNITY_STANDALONE
#define PLIB_WINDOWS
#elif UNITY_ANDROID
#define PLIB_ANDROID
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if PLIB_WINDOWS
using BIXOLON_SamplePg;
using System.Text;
#endif

[ExecuteInEditMode]
public class PrinterAPI : MonoBehaviour
{
    public enum PrinterState { Disconnected, Connected, Connecting, Unaivalable }
    public enum PrinterInterface { Serial, Parallel, USB, LAN, WLAN, Bluetooth }
    public enum TextAlignment { Left, Center, Right }
    public enum TextAttribute
    {
        Default = 0,
        Bold = 2,
        Underline = 4,
        UnderThick = 6,
        Reverse = 8,
        FontB = 1,
        FontC = 16,
        Red = 64
    }
    public enum TextSize
    {
        X1 = 0,
        X2 = 17,
        X3 = 34,
        X4 = 51,
        X5 = 68,
        X6 = 85,
        X7 = 102,
        X8 = 119,
        W2 = 16,
        W3 = 32,
        H2 = 1,
        H3 = 2,
    }
    public enum CharSet
    {
        USER = 19,
        UTF8 = 65001,
        US_EU = 0,
        LATIN_1 = 12,
        VIETNAMESE = 41,
        KOREAN = 949
    }
    public enum CharSetInternational
    {
        USA,
        France,
        Germany,
        UK,
        Denmark1,
        Sweden,
        Italy,
        Spain,
        Japan,
        Norway,
        Denmark2,
        Spain2,
        Latin,
        Korea,
        Slovenia,
        China
    }

    public class PrinterConfig
    {
        public PrinterInterface comInterface;
        public string ip = string.Empty;
        public int port = 0;
        public int baudRate = 0;
        public int dataBits = 0;
        public int parity = 0;
        public int stopBits = 0;

        public int ComInterface
        {
            get
            {
                return (int)comInterface;
            }
        }
        public string PortName
        {
            get
            {
                switch (comInterface)
                {
                    case PrinterInterface.Serial:
                        return ("COM" + port);
                    case PrinterInterface.Parallel:
                        return ("LPT" + port);
                    case PrinterInterface.LAN:
                        return ip;
                    case PrinterInterface.WLAN:
                        return ip;
                    case PrinterInterface.Bluetooth:
                        return ("COM" + port);
                    default:
                        return string.Empty;
                }
            }
        }
        public int BaudRate
        {
            get
            {
                switch (comInterface)
                {
                    case PrinterInterface.Serial:
                        return baudRate;
                    case PrinterInterface.LAN:
                        return port;
                    case PrinterInterface.WLAN:
                        return port;
                    default:
                        return 0;
                }
            }
        }
        public int DataBits
        {
            get
            {
                switch (comInterface)
                {
                    case PrinterInterface.Serial:
                        return DataBits;
                    default:
                        return 0;
                }
            }
        }
        public int Parity
        {
            get
            {
                switch (comInterface)
                {
                    case PrinterInterface.Serial:
                        return parity;
                    default:
                        return 0;
                }
            }
        }
        public int StopBits
        {
            get
            {
                switch (comInterface)
                {
                    case PrinterInterface.Serial:
                        return StopBits;
                    default:
                        return 0;
                }
            }
        }
    }

    public class PrintSection
    {
        public string text = string.Empty;
        public TextAlignment alignment = TextAlignment.Left;
        public TextAttribute[] attributes = new PrinterAPI.TextAttribute[] { PrinterAPI.TextAttribute.Default };
        public TextSize size = TextSize.X1;
        public CharSet charSet = CharSet.UTF8;
        public bool newLine = true;

        public string Text
        {
            get
            {
                if (newLine) return (text + "\n");
                return text;
            }
        }

        public int Attributes
        {
            get
            {
                int a = 0;
                for (int i = 0; i < attributes.Length; i++)
                {
                    a = (a | (int)(attributes[i]));
                }
                return a;
            }
        }
    }

    public static PrinterConfig config = new PrinterConfig() { comInterface = PrinterInterface.USB };
    public static PrinterState state;
    public static event Action StateUpdated;
    static void UpdateState(PrinterState s)
    {
        state = s;
        if (StateUpdated != null) StateUpdated();
    }

    public static bool Connect()
    {
        //return true;

        if (state == PrinterState.Connected)
        {
            return true;
        }

        if (state == PrinterState.Disconnected)
        {
            int r = -1;
#if PLIB_WINDOWS
            r = BXLAPI.PrinterOpen(
                config.ComInterface,
                config.PortName,
                config.BaudRate,
                config.DataBits,
                config.Parity,
                config.StopBits);
#elif PLIB_ANDROID

#endif

            if (r == 0)
            {
                UpdateState(PrinterState.Connected);
                return true;
            }
            else
            {
                UpdateState(PrinterState.Unaivalable);
                return false;
            }
        }
        return false;
    }

    public static void Disconnect()
    {
        //return;

#if PLIB_WINDOWS
        if (state == PrinterState.Connected)
        {
            BXLAPI.PrinterClose();
        }
#endif
    }

    public static string Print(params PrintSection[] sections)
    {
        return Print(CharSet.LATIN_1, CharSetInternational.USA, sections);
    }
    public static string Print(CharSet charSet, CharSetInternational charSetI, params PrintSection[] sections)
    {
#if UNITY_EDITOR
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < sections.Length; i++)
        {
            sb.Append(sections[i].Text);
        }
        sb.Append("\n");
        UILogger.Log(sb.ToString());
        return "editor";
#endif

        if (sections == null || sections.Length == 0)
        {
            return "no_data_to_print";
        }

        string res = string.Empty;
#if PLIB_WINDOWS
        if (Connect())
        {
            try
            {
                BXLAPI.TransactionStart();
                BXLAPI.InitializePrinter();
                BXLAPI.SetCharacterSet((int)charSet);
                BXLAPI.SetInterChrSet((int)charSetI);
                for (int i = 0; i < sections.Length; i++)
                {
                    PrintSection section = sections[i];
                    BXLAPI.PrintTextW(section.Text, (int)section.alignment, section.Attributes, (int)section.size, (int)section.charSet);
                }
                BXLAPI.CutPaper();
                // Leaves 'Transaction' mode, and then sends print data in the buffer to start printing.
                if (BXLAPI.TransactionEnd(true, 3000 /* 3 seconds */) != BXLAPI.BXL_SUCCESS)
                {
                    // failed to read a response from the printer after sending the print-data.
                    res = "printer_did_not_respond";
                }
            }
            catch (Exception e)
            {
                res = e.Message;
            }
        }
        else
        {
            res = "printer_not_connected";
        }
#endif

        return res;
    }

    public static int InitializePrinter()
    {
#if PLIB_WINDOWS
        return BXLAPI.InitializePrinter();
#else
        return 0;
#endif
    }
    public static int SetCharacterSet(int cs)
    {
#if PLIB_WINDOWS
        return BXLAPI.SetCharacterSet(cs);
#else
        return 0;
#endif
    }
    public static int GetCharacterSet()
    {
#if PLIB_WINDOWS
        return BXLAPI.GetCharacterSet();
#else
        return 0;
#endif
    }
}