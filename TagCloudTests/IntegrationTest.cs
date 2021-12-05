﻿using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Analyzers;
using TagCloud.Creators;
using TagCloud.Layouters;
using TagCloud.UI.Console;
using TagCloud.Visualizers;
using TagCloud.Writers;
using TextReader = TagCloud.Readers.TextReader;

namespace TagCloudTests
{
    public class IntegrationTest
    {
        [Test]
        public void ClientRun_ShouldCreateFileWithTagCloud()
        {
            var args = new[] {"-i", "test.txt", "-o", "test.png"};
            var fileReader = new TextReader();
            var textAnalyzer = new TextAnalyzer();
            var freqAnalyzer = new FrequencyAnalyzer();
            var tagCreator = new TagCreatorFactory();
            var layouterFactory = new CircularCloudLayouterFactory();
            var visualizer = new CloudVisualizer();
            var fileWriter = new BitmapWriter();
            var fileInfo = new FileInfo(Environment.CurrentDirectory + "\\test.txt");
            using (var writer = fileInfo.CreateText())
                writer.Write("test\ntext\ntest\ntext\na\nb");
            var fileWithBoringWords = new FileInfo(Environment.CurrentDirectory + "\\excluded.txt");
            using (var writer = fileWithBoringWords.CreateText())
                writer.Write("a\nb");


            var client = new ConsoleUI(fileReader, 
                textAnalyzer, 
                freqAnalyzer, 
                layouterFactory, 
                visualizer, 
                fileWriter, 
                tagCreator);
            client.Run(args);

            var image = new FileInfo(Environment.CurrentDirectory + "\\test.txt");
            image.Exists.Should().BeTrue();
            image.Delete();
            fileInfo.Delete();
            fileWithBoringWords.Delete();
        }
    }
}
