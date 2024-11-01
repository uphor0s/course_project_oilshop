using System.Data;


namespace OilShopApp.Pages;

public partial class ManagerMainPage : ContentPage
{
    Global global = new Global();
    string table = "employees";

    Action<System.Object, System.EventArgs> refreshFunc;

    List<Classes.GenericObject> employees;
    List<Classes.GenericObject> clients;
    List<Classes.GenericObject> cars;
    List<Classes.GenericObject> performed_operations;
    List<Classes.GenericObject> positions;
    List<Classes.GenericObject> operations;


    Page newElement;
    List<string> edit_data;


    public ManagerMainPage()
    {
        InitializeComponent();
        employeeListBtn_Clicked(null, null);

        
    }
    

    void employeeListBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        table = "employees";
        refreshFunc = employeeListBtn_Clicked;

        employees = new List<Classes.GenericObject>();
        var (reader, connection) = global.sqlReader($"select id, surname, name, patronymic, phone, nickname, position_id from employees");
        while (reader.Read())
        {
            employees.Add(new Classes.GenericObject());
            employees[employees.Count - 1].Data = new List<string>();
            for (int i = 0; i < 7; i++) employees[employees.Count - 1].Data.Add(reader.GetValue(i).ToString());
            employees[employees.Count - 1].DisplayName =
                $"{employees[employees.Count - 1].Data[1]} {employees[employees.Count - 1].Data[2]} {employees[employees.Count - 1].Data[3]}";

            var (reader_, connection_) = global.sqlReader($"select name from positions where id={Convert.ToInt16(employees[employees.Count - 1].Data[6])}");
            if(reader_.Read())
            employees[employees.Count - 1].Description1 =
                reader_.GetString(0) + $" ({employees[employees.Count - 1].Data[6]})";
            reader_.Close();
            connection_.Close();

            employees[employees.Count - 1].Description2 = "+7" + employees[employees.Count - 1].Data[4];
            employees[employees.Count - 1].Additional1 = employees[employees.Count - 1].Data[5];
            employees[employees.Count - 1].Additional2 = employees[employees.Count - 1].Data[0];
        }

        listView.Content = new Views.ManagerListView(employees, employeeListBtn_Clicked, table);
        //((CollectionView)listView.Content.FindByName("collection")).SelectedItem.
        reader.Close();
        connection.Close();

    }

    void clientListBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        //newElement = new AddUserPage(null, false);
        table = "clients";
        //refreshFunc = clientListBtn_Clicked;

        clients = new List<Classes.GenericObject>();
        var (reader, connection) = global.sqlReader($"select id, surname, name, patronymic, phone, nickname from clients");
        while (reader.Read())
        {
            clients.Add(new Classes.GenericObject());
            clients[clients.Count - 1].Data = new List<string>();
            for (int i = 0; i < 6; i++) clients[clients.Count - 1].Data.Add(reader.GetValue(i).ToString());
            clients[clients.Count - 1].DisplayName =
                $"{clients[clients.Count - 1].Data[1]} {clients[clients.Count - 1].Data[2]} {clients[clients.Count - 1].Data[3]}";

            clients[clients.Count - 1].Description1 = "+7" + clients[clients.Count - 1].Data[4];
            clients[clients.Count - 1].Additional1 = clients[clients.Count - 1].Data[5];
            clients[clients.Count - 1].Additional2 = clients[clients.Count - 1].Data[0];
        }

        listView.Content = new Views.ManagerListView(clients, clientListBtn_Clicked, table);

        
        //((CollectionView)listView.Content.FindByName("collection")).SelectedItem.
        reader.Close();
        connection.Close();

    }

    void billListBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        table = "performed_operations";
        //refreshFunc = clientListBtn_Clicked;
        
        performed_operations = new List<Classes.GenericObject>();
        var (reader, connection) = global.sqlReader($"select id, client_id, car_id, employee_id, operation_id, date from performed_operations");
        

        while (reader.Read())
        {
            performed_operations.Add(new Classes.GenericObject());
            performed_operations[performed_operations.Count - 1].Data = new List<string>();
            for (int i = 0; i < 6; i++) performed_operations[performed_operations.Count - 1].Data.Add(reader.GetValue(i).ToString());
            

            var (reader_, connection_) = global.sqlReader($"select operations.name, date,employees.surname, employees.name ,employees.patronymic, clients.surname, clients.name, clients.patronymic,cars.model, cars.license_plate from employees,performed_operations, operations, clients, cars where employees.id = performed_operations.employee_id and operations.id = performed_operations.operation_id and performed_operations.client_id = clients.id and performed_operations.car_id = cars.id");
            

            var display_list = new List<List<string>>();
            while (reader_.Read())
            {
                

                display_list.Add(new List<string>());
                for (int i = 0; i < 10; i++) display_list[display_list.Count-1].Add(reader_.GetValue(i).ToString());
            }
            reader_.Close();
            connection_.Close();
            

            performed_operations[performed_operations.Count - 1].Additional2 = display_list[performed_operations.Count - 1][9];
            performed_operations[performed_operations.Count - 1].DisplayName =
                $"{display_list[performed_operations.Count - 1][0]} ({display_list[performed_operations.Count - 1][1]})";
            

            performed_operations[performed_operations.Count - 1].Description1 = $"{display_list[performed_operations.Count - 1][2]} {display_list[performed_operations.Count - 1][3][0]}. {display_list[performed_operations.Count - 1][4][0]}.";
            performed_operations[performed_operations.Count - 1].Description2 = $"{display_list[performed_operations.Count - 1][5]} {display_list[performed_operations.Count - 1][6]} {display_list[performed_operations.Count - 1][7]}";
            performed_operations[performed_operations.Count - 1].Additional1 = display_list[performed_operations.Count - 1][8];
            


        }

        listView.Content = new Views.ManagerListView(performed_operations, billListBtn_Clicked, table);

        //marina_v_d

        //((CollectionView)listView.Content.FindByName("collection")).SelectedItem.
        reader.Close();
        connection.Close();
    }

    void carListBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        table = "cars";
        //refreshFunc = clientListBtn_Clicked;

        cars = new List<Classes.GenericObject>();
        var (reader, connection) = global.sqlReader($"select id, model, year, license_plate from cars");
        while (reader.Read())
        {
            cars.Add(new Classes.GenericObject());
            cars[cars.Count - 1].Data = new List<string>();
            for (int i = 0; i < 4; i++) cars[cars.Count - 1].Data.Add(reader.GetValue(i).ToString());
            cars[cars.Count - 1].DisplayName =
                $"{cars[cars.Count - 1].Data[1]}";

            cars[cars.Count - 1].Description1 = cars[cars.Count - 1].Data[2];
            cars[cars.Count - 1].Additional1 = cars[cars.Count - 1].Data[3];
            cars[cars.Count - 1].Additional2 = cars[cars.Count - 1].Data[0];
        }

        listView.Content = new Views.ManagerListView(cars, carListBtn_Clicked, table);


        //((CollectionView)listView.Content.FindByName("collection")).SelectedItem.
        reader.Close();
        connection.Close();
        
    }

    void positionListBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        table = "positions";
        //refreshFunc = clientListBtn_Clicked;

        positions = new List<Classes.GenericObject>();
        var (reader, connection) = global.sqlReader($"select id, name, salary from positions");
        while (reader.Read())
        {
            positions.Add(new Classes.GenericObject());
            positions[positions.Count - 1].Data = new List<string>();
            for (int i = 0; i < 3; i++) positions[positions.Count - 1].Data.Add(reader.GetValue(i).ToString());
            positions[positions.Count - 1].DisplayName = positions[positions.Count - 1].Data[1];

            positions[positions.Count - 1].Description1 = $"{positions[positions.Count - 1].Data[2]} руб.";
            positions[positions.Count - 1].Additional1 = $"id:{positions[positions.Count - 1].Data[0]}";
        }

        listView.Content = new Views.ManagerListView(positions, positionListBtn_Clicked, table);


        //((CollectionView)listView.Content.FindByName("collection")).SelectedItem.
        reader.Close();
        connection.Close();
    }

    void operationListBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        table = "operations";
        //refreshFunc = clientListBtn_Clicked;

        operations = new List<Classes.GenericObject>();
        var (reader, connection) = global.sqlReader($"select id, name, cost from operations");
        while (reader.Read())
        {
            operations.Add(new Classes.GenericObject());
            operations[operations.Count - 1].Data = new List<string>();
            for (int i = 0; i < 3; i++) operations[operations.Count - 1].Data.Add(reader.GetValue(i).ToString());
            operations[operations.Count - 1].DisplayName = operations[operations.Count - 1].Data[1];

            operations[operations.Count - 1].Description1 = $"{operations[operations.Count - 1].Data[2]} руб.";
            operations[operations.Count - 1].Additional1 = $"id:{operations[operations.Count - 1].Data[0]}";
        }

        listView.Content = new Views.ManagerListView(operations, operationListBtn_Clicked, table);


        //((CollectionView)listView.Content.FindByName("collection")).SelectedItem.
        reader.Close();
        connection.Close();
    }
}

    
