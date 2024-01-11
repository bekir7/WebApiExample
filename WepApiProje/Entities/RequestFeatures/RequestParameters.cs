using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
	public abstract class RequestParameters
	{
		const int maxPageSize = 50;

		//auto-implemented prop
        public int PageNumber { get; set; }
		private int pageSize;
		//full prop
		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value > maxPageSize ? maxPageSize : value; }//maxpagesizedan büyükse maxpage size ver değilse değeri ver
		    
		}

        public String? OrderBy { get; set; }
        public String? Fields { get; set; }

    }
}
