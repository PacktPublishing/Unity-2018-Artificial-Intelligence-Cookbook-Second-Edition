public class Seek : AgentBehavior
{
  public override Steering GetSteering()
  {
    Steering steering = new Steering();
    steering.linear = target.transform.position - transform.position;
    steering.linear.Normalize();
    steering.linear = steering.linear * agent.maxAccel;
    return steering;
  }
}
