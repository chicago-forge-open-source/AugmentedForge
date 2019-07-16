public class Marker
{
    private string _label;
    private float _x;
    private float _z;
    
    public Marker(string label, float x, float z)
    {
        _label = label;
        _x = x;
        _z = z;
    }
    
    public string label => _label;

    public float x => _x;

    public float Z => _z;
}
