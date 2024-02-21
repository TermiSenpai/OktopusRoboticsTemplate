public interface ISensorEventHandler
{
    void OnSensorDetected(string plcCode, bool detectionResult);
}
