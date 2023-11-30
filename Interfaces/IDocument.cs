using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents_Galkin.Interfaces
{
	public interface IDocument
	{
		void Save(bool Update = false);

		List<Documents_Galkin.classes.DocumentContext> AllDocuments();
		void Delete();
	}
}
