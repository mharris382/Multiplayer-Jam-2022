using Godot;
using System;

public class SteamSource : Node2D
{
	[Signal]
	delegate void SteamSourceChanged(Vector2 world_position, int output);
	
	[Export]
	private int _sourceOutput = 3;
	
	[Export]
	private bool _sourceStartsEnabled = true;
	private bool _sourceEnabled;


	public int Output {
		get => Enabled ? _sourceOutput : 0;
		set {
			if(_sourceOutput != value){
				_sourceOutput = value;
				if(_sourceEnabled)
					BroadcastSourceStateChanged();
			}
		}
	}
	public bool Enabled{
		get => _sourceEnabled && _sourceOutput > 0;
		set {
			if (value != _sourceEnabled)
			{
				_sourceEnabled = value;
				BroadcastSourceStateChanged();
			}
		}
	}
	
	
	public override void _Ready()
	{
		_sourceEnabled = _sourceStartsEnabled;
		if (_sourceStartsEnabled){
			BroadcastSourceStateChanged();
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	void BroadcastSourceStateChanged()
	{
		EmitSignal("SteamSourceChanged", Position, Enabled ? _sourceOutput : 0);
	}

	void _on_button_down()
	{
		GD.Print("button callback triggered");
		Enabled = !Enabled;
	}
}
