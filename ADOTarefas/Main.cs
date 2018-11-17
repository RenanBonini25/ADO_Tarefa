using System;
using System.Collections.Generic;
using ADOTarefas;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Widget;


namespace ListTarefas
{
	[Activity(Label = "Main", MainLauncher = true)]
	public class Main : Activity
	{
		ArrayAdapter<string> adapter;
		List<Tarefa> listaTarefas;
		List<string> nomes = new List<string>();
		ListView lista;
		private static readonly int botaoNotificacao = 0;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.main);

			Button botaoAdd = FindViewById<Button>(Resource.Id.botaoAdd);
			Button botaoExcluir = FindViewById<Button>(Resource.Id.botaoExcluir);
			lista = FindViewById<ListView>(Resource.Id.listaTarefas);
			lista.ChoiceMode = ChoiceMode.Single;

			if (Tarefa.listaTarefas == null)
			{
				listaTarefas = new List<Tarefa>();
			}
			else
			{
				listaTarefas = Tarefa.listaTarefas;
				nomes = nomesTarefas(listaTarefas);
				enviarMensagem(listaTarefas);
				

			}
			
			adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemSingleChoice, nomesTarefas(listaTarefas));
			lista.Adapter = adapter;

			lista.ItemLongClick += delegate {
				AlertDialog.Builder caixa = new AlertDialog.Builder(this);
				caixa.SetTitle("Descrição");
				caixa.SetMessage("Teste");
				caixa.SetPositiveButton("OK", (sender, args) =>
				{
				});
				caixa.Show();
			};

			try
			{
				bool enviado = Intent.Extras.GetBoolean("enviado");
				if (enviado)
				{
					CheckarItem();
				}
			}
			catch (Exception ex) {
			}

			botaoAdd.Click += delegate
			{
				StartActivity(typeof(AddTarefa));
			};

			botaoExcluir.Click += delegate
			{
				var posicao = lista.CheckedItemPosition;
				if (lista.CheckedItemCount != 0)
				{
					excluirTarefa(posicao);
				}
				else
				{
					AlertDialog.Builder caixa = new AlertDialog.Builder(this);
					caixa.SetTitle("Excluir Tarefa");
					caixa.SetMessage("Selecione uma tarefa");
					caixa.SetPositiveButton("OK", (sender, args) =>
					{
					});
					caixa.Show();
				}

			};

			List<string> nomesTarefas (List<Tarefa> tarefas) {
				List<string> nomes = new List<string>();
				foreach (Tarefa tarefa in tarefas) {
					nomes.Add(tarefa.nomeTarefa + " (" + tarefa.dataTarefa + ")");
				}
				return nomes;
			}

			void CheckarItem() {
				for (int i = 0; i < lista.Count; i++) {
					if (lista.GetItemAtPosition(i).ToString().Contains(DateTime.Now.ToString("dd/MM/yyyy"))) {
						lista.SetItemChecked(i, true);
					}
				}

			}
			
			void excluirTarefa(int posicao)
			{
				AlertDialog.Builder caixa = new AlertDialog.Builder(this);
				caixa.SetTitle("Excluir Tarefa");
				caixa.SetCancelable(true);
				caixa.SetMessage("Deseja excluir a tarefa?");
				caixa.SetPositiveButton("Excluir", (sender, args) =>
				{
					Tarefa.listaTarefas.RemoveAt(posicao);
					StartActivity(typeof(Main));
				});
				caixa.Show();
			}

			void enviarMensagem(List<Tarefa> tarefas) {
				foreach (Tarefa tarefa in tarefas) {
					if (tarefa.dataTarefa.Equals(DateTime.Now.ToString("dd/MM/yyyy"))) {
						enviarNotificacao();
					}
				}
			}

			void enviarNotificacao() {
				Intent migrar = new Intent(this, typeof(Main));
				migrar.PutExtra("enviado", true);
				Android.Support.V4.App.TaskStackBuilder stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(this);
				stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(Main)));
				stackBuilder.AddNextIntent(migrar);

				PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int) PendingIntentFlags.UpdateCurrent);

				NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
				.SetAutoCancel(true)
				.SetContentIntent(resultPendingIntent)
				.SetContentTitle("Dia da tarefa")
				.SetSmallIcon(Resource.Mipmap.ic_launcher)
				.SetContentText("O dia da sua tarefa chegou! Clique para vê-la!");

				NotificationManager notificationManager =
				(NotificationManager)GetSystemService(Context.NotificationService);
				notificationManager.Notify(botaoNotificacao, builder.Build());
			}
		}
	}
}