using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_PermanentForm.DB
{
    class ProcessInfoFormat
    {
        private DateTime _buildDate;
        public DateTime BuildDate
        {
            get { return _buildDate; }
            set { _buildDate = value; }
        }

        private int _layerCnt;
        public int LayerCnt
        {
            get { return _layerCnt; }
            set { _layerCnt = value; }
        }

        private string _jobName;
        public string JobName
        {
            get { return _jobName; }
            set { _jobName = value; }
        }

        private double _layerThichness;
        public double LayerThickness
        {
            get { return _layerThichness; }
            set { _layerThichness = value; }
        }

        private string _materialType;
        public string MaterialType
        {
            get { return _materialType; }
            set { _materialType = value; }
        }

    }
}
