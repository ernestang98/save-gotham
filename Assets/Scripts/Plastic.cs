public class Plastic : InteractableObject
{
    private int pointsToAdd = 10;
    private int pointsToDeduct = 10;

    private const string plasticText = "Plastic";

    public override void AddPoints(string tag)
    {
        if (tag == plasticText)
            pointsController.addPoints(pointsToAdd);
        else
            pointsController.addPoints(-1 * pointsToDeduct);
        Destroy(this.gameObject);
    }
}