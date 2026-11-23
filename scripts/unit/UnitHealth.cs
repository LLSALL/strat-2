using Godot;
using System;

public partial class Unit : Node3D
{
	private float _health = 100.0f;
	public float Health
	{
		get => _health;
		set
		{
			_health = value;
			if (_health <= 0)
			{
				this.QueueFree();
			}
		}
	}
}
