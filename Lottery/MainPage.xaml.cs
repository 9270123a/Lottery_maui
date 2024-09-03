using System.Drawing;
using Microsoft.Maui.Layouts;
using 模擬大樂透開獎;
using static System.Net.Mime.MediaTypeNames;


namespace Lottery;

public partial class MainPage : ContentPage
{
	int count = 0;
    public MainPage()
	{
		InitializeComponent();
        CreateDynamicButtons();

    }
    private List<Button> selectedNumbers = new List<Button>();
    ParameterControll parameterControl = new ParameterControll();



    private void CreateDynamicButtons()
    {
        for (int i = 0; i < 49; i++)
        {
            Button button = new Button
            {
                Text = $"{i + 1}",
                WidthRequest = 100, // 設置固定寬度
                HeightRequest = 50, // 設置固定高度
                Margin = new Thickness(5) // 添加一些間距
            };

            button.Clicked += Button_Click;
            buttonContainer.Children.Add(button);
        }
    }
    private void Button_Click(object sender, EventArgs e)
    {
        if (sender is Button clickedButton)
        {
            (bool MesseageShow, string Message) = parameterControl.ToggleButtonSelection(clickedButton.Text);

            if (MesseageShow)
            {
                DisplayAlert("提示訊息", Message, "OK");
                return;
            }

            if (clickedButton.BackgroundColor == Colors.Yellow)
            {
                clickedButton.BackgroundColor = Colors.White;
            }
            else
            {
                clickedButton.BackgroundColor = Colors.Yellow;
            }
            UpdateDisplay();

        }


    }


    private void Return_Click(object sender, EventArgs e)
    {
        if (selectedNumbers.Count > 0)
        {
            var result = parameterControl.remove();
            if (!result.Item1)
            {
                DisplayAlert("錯誤", result.Item2, "確定");
                return;
            }
            UpdateDisplay();
        }


    }

    private void computer_Click(object sender, EventArgs e)
    {
        parameterControl.ComputerPick();
        UpdateDisplay();
    }
    private List<Button> FindButtonsByNumbers(string numbers)
    {
        

        List<string> numberList = numbers.Split(',').ToList();
        return buttonContainer.Children.OfType<Button>()
            .Where(btn => numberList.Contains(btn.Text))
            .ToList();
    }
    private void ClearAllSelections()
    {
        foreach (Button btn in selectedNumbers)
        {
            btn.BackgroundColor = Colors.White;
        }
        selectedNumbers.Clear();
    }

    public void HighlightSelectedNumbers(List<Button> buttons)
    {
        foreach (Button btn in buttons)
        {
            btn.BackgroundColor = Colors.Yellow;
        }
    }

    private void UpdateDisplay()
    {
        ClearAllSelections();
        string selectedButtonTexts = parameterControl.GetSelectedNumbers();
        selectedNumbers = FindButtonsByNumbers(selectedButtonTexts);
        HighlightSelectedNumbers(selectedNumbers);
        ResultEntry1.Text = selectedButtonTexts;

    }
    private void PrizeButton_Click(object sender, EventArgs e)
    {
        var result = parameterControl.Lottery_draw();
        parameterControl.Generate_winingnumber();

        if (result.Item1)
        {
            DisplayAlert("錯誤", result.Item2, "確定");
        }
        else
        {
            ResultEntry2.Text = result.Item2;
            PrizeResult.Text = result.Item3;
        }

    }

}

