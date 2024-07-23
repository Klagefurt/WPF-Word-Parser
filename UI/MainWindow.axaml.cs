using System;
using Avalonia.Controls;

namespace TableParser.UI;

public partial class MainWindow : Window
{
	private readonly TextBox textBox;
	private readonly TextBlock textBlock;
	private readonly string title = "Result of parser's work:";

	public MainWindow()
	{
		InitializeComponent();
		textBox = this.Find<TextBox>("TextBox");
		textBlock = this.Find<TextBlock>("TextBlock");

		textBox.Text = " d ";
		textBox.PropertyChanged += (sender, e) =>
		{
			textBlock.Text =
				title
				+ Environment.NewLine + Environment.NewLine
				+ string.Join(Environment.NewLine, FieldsParserTask.ParseLine(textBox.Text));
		};
	}
}