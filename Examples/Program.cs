using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml;
using com.mxgraph;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            example1();
            example2();
            example5();

            mxCodecTest();
            //Console.ReadKey();
        }

        static void example1()
        {
            // Creates graph with model
            mxGraph graph = new mxGraph();
            Object parent = graph.GetDefaultParent();

            var doc = mxUtils.ParseXml(mxUtils.ReadFile(@"..\..\..\img\moon.xml"));
            var codec = new mxCodec(doc);
            var stencil = codec.Decode(doc.FirstChild);
            var stencil1 = new mxStencil(doc.DocumentElement);

            // Adds cells into the graph
            graph.Model.BeginUpdate();
            try
            {
                var state = graph.View.States.Values.FirstOrDefault();
                //stencil1.PaintShape(graph, state);
                //parent.
                //    .DrawState()
                Object v1 = graph.InsertVertex(parent, null, "Hello", 20, 20, 80, 30);
                Object v2 = graph.InsertVertex(parent, null, "World!", 200, 150, 80, 30);
                Object e1 = graph.InsertEdge(parent, null, "e1", v1, v2);
            }
            finally
            {
                graph.Model.EndUpdate();
            }

            mxGraph graph1 = new mxGraph();
            Image img0 = mxCellRenderer.CreateImage(graph1, new object[]{ stencil }, 1, Color.White, true, new mxRectangle(0, 0, 150, 200));
            img0.Save("example0.png", ImageFormat.Png);

            // Example to save the graph in multiple images
            Image img = mxCellRenderer.CreateImage(graph, null, 1, Color.White, true, new mxRectangle(0, 0, 150, 200));
            img.Save("example1.png", ImageFormat.Png);

            Image img2 = mxCellRenderer.CreateImage(graph, null, 1, Color.Transparent, true, new mxRectangle(150, 0, 150, 200));
            img2.Save("example2.png", ImageFormat.Png);

            Image img3 = mxCellRenderer.CreateImage(graph, null, 1, Color.Transparent, true, new mxRectangle(0, 0, 300, 200));
            img3.Save("example3.png", ImageFormat.Png);

            Image img4 = mxCellRenderer.CreateImage(graph, null, 1, Color.White, true, new mxRectangle(0, 0, 300, 200));
            img4.Save("example4.png", ImageFormat.Png);
        }

        static void example2()
        {
            XmlDocument doc = mxUtils.ParseXml(mxUtils.ReadFile(@"..\..\..\img\basic.xml"));
            XmlNodeList list = doc.DocumentElement.GetElementsByTagName("shape");

            for (int i = 0; i < list.Count; i++)
            {
                XmlElement shape = (XmlElement)list.Item(i);
                mxStencilRegistry.AddStencil(shape.GetAttribute("name"),
                        new mxStencil(shape));
            }

            mxGraph graph = new mxGraph();
            Object parent = graph.GetDefaultParent();
            // Adds cells into the graph
            graph.Model.BeginUpdate();
            try
            {
                Object v2 = graph.InsertVertex(parent, null, "", 1, 1, 62, 62, "shape=Cloud Callout");
            }
            finally
            {
                graph.Model.EndUpdate();
            }

            Image img4 = mxCellRenderer.CreateImage(graph, null, 1, Color.White, true, new mxRectangle(0, 0, 64, 64));
            img4.Save("example7.png", ImageFormat.Png);

            //XmlTextReader xmlReader = new XmlTextReader(new StringReader(mxUtils.ReadFile(@"..\..\..\img\moon.xml")));
            //mxGraphViewImageReader viewReader = new mxGraphViewImageReader(
            //    xmlReader, Color.White, 4, true, false);
            //Image image = mxGraphViewImageReader.Convert(viewReader);
            //image.Save("example5.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        // New image export from file
        static void example5()
        {
            /*XmlTextReader xmlReader = new XmlTextReader(new StringReader(mxUtils.ReadFile("../../../../export.xml")));
            Image image = mxUtils.CreateImage(800, 800, Color.White);
            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.HighQuality;
            mxSaxOutputHandler handler = new mxSaxOutputHandler(new mxGdiCanvas2D(g));
            handler.Read(xmlReader);

            image.Save("C:/example1.png", System.Drawing.Imaging.ImageFormat.Png);*/
        }

        static void mxCodecTest()
        {
            // Creates graph with model
            mxGraph graph = new mxGraph();

            // Adds cells into the model
            Object parent = graph.GetDefaultParent();
            graph.Model.BeginUpdate();
            Object v1, v2, e1;
            try
            {
                v1 = graph.InsertVertex(parent, null, "Hello", 20, 20, 80, 30);
                v2 = graph.InsertVertex(parent, null, "World!", 200, 150, 80, 30);
                e1 = graph.InsertEdge(parent, null, "e1", v1, v2);
            }
            finally
            {
                graph.Model.EndUpdate();
            }

            mxCodec codec = new mxCodec();
            XmlNode node = codec.Encode(graph.Model);
            string xml1 = mxUtils.GetPrettyXml(node);

            codec = new mxCodec();
            Object model = codec.Decode(node);

            codec = new mxCodec();
            node = codec.Encode(model);
            string xml2 = mxUtils.GetPrettyXml(node);

            Console.WriteLine("mxCodecTest Passed: " + (xml1.Equals(xml2)));
        }
    }
}
