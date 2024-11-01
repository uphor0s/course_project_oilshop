using System.Security.Cryptography;
using System.Text;

namespace OilShopApp.Pages;

public partial class AddUserPage : ContentPage
{
    Global global = new Global();
    private string session;
    List<string> user_data;
    bool isUpdate;


    public AddUserPage(List<string> page_data, bool update)
	{
		InitializeComponent();
        if (update)
        {
            user_data = page_data;
            isUpdate = update;
            passport.Text = page_data[0];
            surname.Text = page_data[1];
            name.Text = page_data[2];
            patronymic.Text = page_data[3];
            phone.Text = page_data[4];
            nickname.Text = page_data[5];
            RegBtn.Text = "Обновить данные";
            password.Placeholder = "Необязательно";
            password.PlaceholderColor = Color.FromArgb("#404040");
        }
        
    }

    void RegBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        if(passport.Text.Length != 10 || phone.Text.Length != 10)
        {
            DisplayAlert("Ошибка ввода данных", "Номер телефона и паспорт должны иметь длину в 10 чисел.", "ОК");
            return;
        }
        if (isUpdate)
        {
            if (nickname.Text != null && phone.Text != null &&
            passport.Text != null && name.Text != null && surname.Text != null)
            {
                var (reader, connection) = global.sqlReader($"select count(nickname) from clients where nickname=\'{nickname.Text}\'");
                if (reader.Read())
                {
                    if (nickname.Text !=user_data[5] && reader.GetInt16(0) != 0)
                    {
                        DisplayAlert("Ошибка ввода данных", "Введенный логин недоступен.", "ОК");
                        return;
                    }
                }
                reader.Close();
                connection.Close();
                (reader, connection) = global.sqlReader($"select count(id) from clients where id=\'{passport.Text}\'");
                if (reader.Read())
                {
                    if (passport.Text != user_data[0] && reader.GetInt16(0) != 0)
                    {
                        DisplayAlert("Ошибка ввода данных", "Аккаунт с указанными паспортными данными уже существует.", "ОК");
                        return;
                    }
                }
                reader.Close();
                connection.Close();
            }
            else
            {
                DisplayAlert("Ошибка ввода данных", "Все поля, кроме поля \"Отчество\" обязательны к заполнению!", "OK");
                return;
            }

            global.nonQuery($"update clients set nickname = '{nickname.Text}', name = '{name.Text}', surname = '{surname.Text}', patronymic = '{patronymic.Text}'," +
                $"phone = '{phone.Text}', id = '{passport.Text}' where id = '{user_data[0]}'");


            if (password.Text != null)
            {
                global.nonQuery($"update clients set password='{global.getHash(password.Text)}' where id = '{passport.Text}'");
            }
                //global.nonQuery($"update performed_operations set client_id = '{passport.Text}' where client_id = '{user_data[0]}'");

                Navigation.PopAsync();
            return;

        }
        if (nickname.Text != null && password.Text != null && phone.Text != null &&
            passport.Text != null && name.Text != null && surname.Text != null)
        {
            var (reader, connection) = global.sqlReader($"select count(nickname) from clients where nickname=\'{nickname.Text}\'");
            if (reader.Read())
            {
                if (reader.GetInt16(0) != 0)
                {
                    DisplayAlert("Ошибка регистрации", "Введенный логин недоступен.", "ОК");
                    return;
                }
            }
            reader.Close();
            connection.Close();
            (reader, connection) = global.sqlReader($"select count(id) from clients where id=\'{passport.Text}\'");
            if (reader.Read())
            {
                if (reader.GetInt16(0) != 0)
                {
                    DisplayAlert("Ошибка регистрации", "Аккаунт с указанными паспортными данными уже существует.", "ОК");
                    return;
                }
            }
            reader.Close();
            connection.Close();
        }
        else
        {
            DisplayAlert("Ошибка ввода данных", "Все поля, кроме поля \"Отчество\" обязательны к заполнению!", "OK");
            return;
        }


        //StringBuilder passwordHash = new StringBuilder();
        //var hash = SHA256.HashData(Encoding.UTF8.GetBytes(password.Text));
        //foreach (byte b in hash) passwordHash.Append((int)b);
        //DisplayAlert( global.nonQuery($"insert into clients (nickname, password, name, surname, patronymic, phone, passport) values " +
        //        $"(\'{nickname.Text}\' ,\'{global.getHash(password.Text)}\',\'{name.Text}\', \'{surname.Text}\', \'{patronymic.Text}\', \'{phone.Text}\', \'{passport.Text}\')").ToString(), "swag","cock");
        //    Navigation.PushAsync(new ClientMainPage(nickname.Text));
        global.nonQuery($"insert into clients (nickname, password, name, surname, patronymic, phone, id) values " +
            $"(\'{nickname.Text}\' ,\'{global.getHash(password.Text)}\',\'{name.Text}\', \'{surname.Text}\', \'{patronymic.Text}\', \'{phone.Text}\', \'{passport.Text}\')");
        Navigation.PopAsync();
    }

    void openLoginBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        Navigation.PopAsync();
    }
}
