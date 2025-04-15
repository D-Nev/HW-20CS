namespace ConsoleApp1
{
    public interface IHeatingStrategy
    {
        double Heat(double temperature, double area);
    }

    public class GasHeating : IHeatingStrategy
    {
        public double Heat(double temperature, double area)
        {
            return 0.2 * temperature * area;
        }
    }

    public class ElectricHeating : IHeatingStrategy
    {
        public double Heat(double temperature, double area)
        {
            return 0.15 * temperature * area;
        }
    }

    public class SolarHeating : IHeatingStrategy
    {
        public double Heat(double temperature, double area)
        {
            return temperature < 10 ? 0.1 * temperature * area : 0.05 * temperature * area;
        }
    }

    public interface IObserver
    {
        void Update(double temperature);
    }

    public class TemperatureSensor
    {
        private List<IObserver> _observers = new List<IObserver>();
        private double _threshold;
        public TemperatureSensor(double threshold)
        {
            _threshold = threshold;
        }
        public void UpdateTemperature(double newTemperature)
        {
            if (newTemperature < _threshold)
            {
                NotifyObservers(newTemperature);
            }
        }
        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }
        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }
        private void NotifyObservers(double temperature)
        {
            foreach (var observer in _observers)
            {
                observer.Update(temperature);
            }
        }
    }
    public class HeatingSystem : IObserver
    {
        private IHeatingStrategy _heatingStrategy;
        public double Area { get; private set; }

        public HeatingSystem(IHeatingStrategy strategy, double area)
        {
            _heatingStrategy = strategy;
            Area = area;
        }

        public void Update(double temperature)
        {
            double energy = _heatingStrategy.Heat(temperature, Area);
            Console.WriteLine($"Нагрев активируется с помощью {_heatingStrategy.GetType().Name}. Потребляемая энергия: {energy} kWh.");
        }

        public void Change(IHeatingStrategy newStrategy)
        {
            _heatingStrategy = newStrategy;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            TemperatureSensor sensor = new TemperatureSensor(18); 
            HeatingSystem heatingSystem = new HeatingSystem(new GasHeating(), 50); 
            sensor.Attach(heatingSystem);

            sensor.UpdateTemperature(20); 
            sensor.UpdateTemperature(17); 
            sensor.UpdateTemperature(15); 
          
            heatingSystem.Change(new ElectricHeating());
            sensor.UpdateTemperature(16); 
        }
    }
}
