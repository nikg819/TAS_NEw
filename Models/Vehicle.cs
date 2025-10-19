namespace TAS_Test.Models;

public class Vehicle
{
    public int vehicleId { get; set; }
    public string vehicleModel { get; set; }
    public string vehicleColour { get; set; }
    public string kennzeichen { get; set; }

    public Vehicle() {}
    
    public Vehicle(int vehicleId, string vehicleModel, string vehicleColour, string kennzeichen)
    {
        this.vehicleId = vehicleId;
        this.vehicleModel = vehicleModel;
        this.vehicleColour = vehicleColour;
        this.kennzeichen = kennzeichen;
    }
    
}