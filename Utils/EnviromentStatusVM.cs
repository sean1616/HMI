using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_PermanentForm.Utils
{
    public class EnviromentStatus
    {
        private double _pressure;
        public double Pressure
        {
            get { return _pressure; }
            set { _pressure = value;
            }
        }

        private double _cavityGasPressure;
        public double CavityGasPressure
        {
            get { return _cavityGasPressure; }
            set
            {
                _cavityGasPressure = value;
            }
        }

        private double _oxygen;
        public double Oxygen
        {
            get { return _oxygen; }
            set
            {
                _oxygen = value;
            }
        }

        private double _flowRate;
        public double FlowRate
        {
            get { return _flowRate; }
            set { _flowRate = value; }
        }
    }
}
