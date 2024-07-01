using System.Runtime.CompilerServices;
using System.Xml.Linq;

//Создать систему мониторинга и управления для умного дома, которая будет уведомлять пользователей о различных событиях, таких как открытие двери, изменение температуры, утечка воды и т.д.

//Описание:
//Ваша задача — разработать несколько классов, представляющих различные устройства умного дома, и класс SmartHome, который будет управлять этими устройствами
//и уведомлять пользователей о событиях. Устройства должны поддерживать соответствующие события, а класс SmartHome должен подписываться на них и обрабатывать их.

//Требования:
//Класс Door:

//Свойства: IsOpen(bool).
//Событие: DoorOpened(возникает при открытии двери).
//Класс Thermostat:

//Свойства: Temperature(double).
//Событие: TemperatureChanged(возникает при изменении температуры).
//Класс WaterSensor:

//Свойства: IsLeakDetected(bool).
//Событие: WaterLeakDetected(возникает при обнаружении утечки воды).
//Класс User:

//Свойства: Name, Email.
//Метод ReceiveNotification(string message): выводит на экран уведомление для пользователя.
//Класс SmartHome:

//Свойства: Users(список пользователей), Devices(список устройств).
//Методы:
//AddUser(User user): добавляет пользователя в систему.
//AddDevice(object device): добавляет устройство в систему.
//SubscribeToEvents(): подписывается на события всех устройств.
//NotifyUsers(string message): отправляет уведомления всем пользователям.
//Дополнительные требования:
//Инкапсуляция и абстракция:

//Все устройства должны иметь общий интерфейс IDevice с методом SubscribeEvents(SmartHome smartHome), который будет подписывать события устройства на обработчики в SmartHome.
//Реализация событий:

//Все события должны быть реализованы с использованием делегатов и событий C#.





User user1 = new User("Vasya", "vvv@gmail.com");
User user2 = new User("Petya", "vvv@gmail.com");

SmartHome house = new SmartHome();
house.AddUser(user1);
house.AddUser(user2);

Door door1 = new Door();

Door door2 = new Door();

Thermostat thermostat = new Thermostat();


WaterSensor water = new WaterSensor();


house.AddDevice(door1);
house.AddDevice(door2);
house.AddDevice(thermostat);
house.AddDevice(water);

foreach (IDevice d in house.devices)
    d.SubscribeEvents(house);

door1.IsOpen = true;
door2.IsOpen = false;
thermostat.Temperature = 25.0;
water.IsLeakDetected = false;

public interface IDevice
{
    public void SubscribeEvents(SmartHome smartHome);
}
public class Door : IDevice
{
    private bool isOpen;
    public bool IsOpen
    {
        get { return isOpen; }
        set
        {
                isOpen = value;
                OnDoorOpened();
        }
    }
    public delegate void DoorOpenHandler(string mes);
    public event DoorOpenHandler DoorOpened;

    protected virtual void OnDoorOpened()
    {
        var msg = IsOpen ? "Дверь открыта" : "Дверь закрыта";
        DoorOpened?.Invoke(msg);
    }

    public void SubscribeEvents(SmartHome smartHome)
    {
        DoorOpened += smartHome.SubscribeToEvents;
    }
}

public class Thermostat : IDevice
{
    private double temperature;
    public double Temperature
    {
        get { return temperature; }
        set
        {
                temperature = value;
                OnTemperatureChanged();
          
        }
    }
    public delegate void ThermostatHandler(string mes);
    public event ThermostatHandler TemperatureChanged;

    protected virtual void OnTemperatureChanged()
    {

        TemperatureChanged?.Invoke($"температура изменилась на {Temperature}");
    }

    public void SubscribeEvents(SmartHome smartHome)
    {
        TemperatureChanged += smartHome.SubscribeToEvents;
    }
}

public class WaterSensor :IDevice
{
    private bool isLeakDetected;
    public bool IsLeakDetected
    {
        get { return isLeakDetected; }
        set
        {
                isLeakDetected = value;
                OnWaterLeakDetected();
        }
    }
    public delegate void WaterLeakHandler(string mes);
    public event WaterLeakHandler WaterLeakDetected;

    protected virtual void OnWaterLeakDetected()
    {
        var msg = isLeakDetected ? "утечка воды" : "утечки воды нет";

            WaterLeakDetected?.Invoke(msg);
    }
    public void SubscribeEvents(SmartHome smartHome)
    {
        WaterLeakDetected += smartHome.SubscribeToEvents;
    }
}

public class User
{
    public string Name;
    public string Email;

    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }
    public void ReceiveNotification(string message)
    {
        Console.WriteLine($"Пользователь {Name} получил сообщение: {message}");
    }
}

public class SmartHome
{
    List<User> users;
    public List<IDevice> devices;
    public SmartHome()
    {
        users = new List<User>();
        devices = new List<IDevice>();
    }

    public void AddUser(User user)
    {
        users.Add(user);
    }

    public void AddDevice(object device)
    {
        devices.Add(device as IDevice);
    }

    public void SubscribeToEvents(string msg)
    {

        Console.WriteLine(msg);
        NotifyUsers(msg);
    }

    public void NotifyUsers(string message)
    {
        foreach (var user in users)
        {
            user.ReceiveNotification(message);
        }
    }
}