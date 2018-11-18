using System.Collections.Generic;

namespace ADOTarefas
{
	class Tarefa
	{
		public string nomeTarefa { get; set; }
		public string dataTarefa { get; set; }
		public static List<Tarefa> listaTarefas { get; set; }
	}
}