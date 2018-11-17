using System.Collections.Generic;
using System.Text;
using ADOTarefas;
using Android.App;
using Android.OS;
using Android.Widget;

namespace ListTarefas
{
	[Activity(Label = "AddTarefa")]
	public class AddTarefa : Activity
	{
		List<Tarefa> listaTarefas;
		DatePicker datePicker;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.addTarefa);

			EditText tarefa = FindViewById<EditText>(Resource.Id.edittext);
			Button botaoConf = FindViewById<Button>(Resource.Id.botaoConfirmar);
			Button botaoVoltar = FindViewById<Button>(Resource.Id.botaoVoltar);
			datePicker = FindViewById<DatePicker>(Resource.Id.datePicker1);

			if (Tarefa.listaTarefas == null)
			{
				listaTarefas = new List<Tarefa>();
			}
			else
			{
				listaTarefas = Tarefa.listaTarefas;
			}

			botaoConf.Click += delegate {
				AlertDialog.Builder caixa = new AlertDialog.Builder(this);
				caixa.SetTitle("Adicionar Tarefa");
				string nomeTarefa = tarefa.Text.Trim();
				if (nomeTarefa.Equals("")) {
					caixa.SetMessage("Digite o nome da tarefa");
					caixa.SetCancelable(true);
					caixa.SetPositiveButton("OK", (sender, args) =>
					{
					});
					caixa.Show();
					return;
				}

				string nome = tarefa.Text;
				Tarefa novaTarefa = new Tarefa();
				novaTarefa.nomeTarefa = nome;
				string data = getDate();
				novaTarefa.dataTarefa = data;

				listaTarefas.Add(novaTarefa);
				Tarefa.listaTarefas = listaTarefas;

				
				caixa.SetMessage("Tarefa adicionada!");
				caixa.Show();

				StartActivity(typeof(Main));
			};

			botaoVoltar.Click += delegate {
				StartActivity(typeof(Main));
			};

		}

		private string getDate()
		{
			StringBuilder data = new StringBuilder();
			data.Append(datePicker.DayOfMonth + "/" + (datePicker.Month + 1) + "/" + datePicker.Year);
			return data.ToString();
		}
	}
}