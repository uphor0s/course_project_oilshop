namespace OilShopApp.Views;

public partial class ManagerListView : ContentView
{
    Global global = new Global();
    Action<System.Object, System.EventArgs> refreshFunc;
    Page newElement;
    Page updateElement;
    Classes.GenericObject selectedItem;
    string currentTable;

    public ManagerListView(List<Classes.GenericObject> objects, Action<System.Object, System.EventArgs> refresh, string table)
	{
		InitializeComponent();
		collection.ItemsSource = objects;
        refreshFunc = refresh;
        currentTable = table;
        collection.HeightRequest = 700;
        countLabel.Text = $"Кол-во: {objects.Count}";
    }

   

    void editBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        if (selectedItem != null)
        {
            switch (currentTable)
            {
                case "clients": Navigation.PushAsync(new Pages.AddUserPage(selectedItem.Data, true)); break;
                case "employees": Navigation.PushAsync(new Pages.AddEmployeePage(selectedItem.Data, true)); break;
                case "performed_operations": Navigation.PushAsync(new Pages.AddBillPage(selectedItem.Data, true)); break;
                case "cars": Navigation.PushAsync(new Pages.AddCarPage(selectedItem.Data, true)); break;
                case "operations": Navigation.PushAsync(new Pages.AddOperationPage(selectedItem.Data, true)); break;
                case "positions": Navigation.PushAsync(new Pages.AddPositionPage(selectedItem.Data, true)); break;

            }
        }
    }

    void deleteBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        if (selectedItem != null)
        {
            global.nonQuery($"delete from {currentTable} where id=\'{selectedItem.Data[0]}\'");
            refreshFunc.Invoke(null, null);
        }
    }

    void newBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        //Navigation.PushAsync(newElement);
        switch (currentTable)
        {
            case "clients": Navigation.PushAsync(new Pages.AddUserPage(null, false)); break;
            case "employees": Navigation.PushAsync(new Pages.AddEmployeePage(null, false)); break;
            case "performed_operations": Navigation.PushAsync(new Pages.AddBillPage(null, false)); break;
            case "cars": Navigation.PushAsync(new Pages.AddCarPage(null, false)); break;
            case "operations": Navigation.PushAsync(new Pages.AddOperationPage(null, false)); break;
            case "positions": Navigation.PushAsync(new Pages.AddPositionPage(null, false)); break;

        }
        refreshFunc.Invoke(null, null);
    }

    void collection_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        //testLabel.Text = (e.CurrentSelection[0]as Classes.GenericObject).Data[0];
        selectedItem = e.CurrentSelection[0] as Classes.GenericObject;
        //((Classes.GenericObject)e.CurrentSelection).Data[0];
    }

    void refreshBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        refreshFunc.Invoke(null, null);
    }

    void collection_SizeChanged(System.Object sender, System.EventArgs e)
    {
        collection.HeightRequest = 700;
    }
}
