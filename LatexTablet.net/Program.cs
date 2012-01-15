/**
 ** Written by Steven Lovegrove
 ** http://www.doc.ic.ac.uk/~sl203/
 */
 
using System;
using System.Windows.Forms;
using micautLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace LatexTablet.net
{
  class Program
  {
    static MathInputControl ctrl;

    [STAThread]
    static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      ctrl = new MathInputControlClass();
      ctrl.EnableExtendedButtons(true);
      ctrl.EnableAutoGrow(true);
      ctrl.Close += () => Application.ExitThread();
      ctrl.Insert += new _IMathInputControlEvents_InsertEventHandler(ctrl_Insert);
      ctrl.SetCaptionText("Copy LaTeX Formular to Clipboard");
      ctrl.Show();
      Application.Run();
    }

    static void ctrl_Insert(string RecoResult)
    {
      // Convert To LaTeX expression
      try
      {
        StringReader sr = new StringReader(RecoResult);
        StringWriter sw = new StringWriter();
        XPathDocument myXPathDocument = new XPathDocument(sr);
        XslTransform myXslTransform = new XslTransform();
        XmlTextWriter writer = new XmlTextWriter(sw);
        myXslTransform.Load("./mmltex.xsl");
        myXslTransform.Transform(myXPathDocument, null, writer);
        writer.Close();
        string latex = sw.ToString();
        Clipboard.SetData(DataFormats.Text, (Object)latex);
      }
      catch (Exception e)
      {
        Console.WriteLine("Exception: {0}", e.ToString());
      }

      ctrl.Hide();
      Application.Exit();
    }
  }
}



//// Display
//RecoResult = RecoResult.Replace("<m:", "<");
//RecoResult = RecoResult.Replace("/m:", "/");
//try
//{
//  StringReader sr = new StringReader(RecoResult);
//  MathML.MathMLDocument doc = new MathML.MathMLDocument();
//  doc.Load(sr);
//  mathml.MathElement = (MathML.MathMLMathElement)doc.DocumentElement;
//} catch(Exception exp) {
//  MessageBox.Show(exp.Message + "trace: " + exp.StackTrace);
//}