using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseShifting : MonoBehaviour
{
    public enum Phases { DefaultPhase, BluePhase, RedPhase, GreenPhase, YellowPhase }

    public Phases currentPhase;

    void OnCollisionEnter2D(Collision2D collision)
    {
        string collisionLayer = LayerMask.LayerToName(collision.gameObject.layer);

        switch (currentPhase)
        {
            case Phases.DefaultPhase:
                if (collisionLayer == "DefaultPhaseCrystal")
                {
                    currentPhase = Phases.DefaultPhase;
                }
                else if (collisionLayer == "RedPhaseCrystal")
                {
                    currentPhase = Phases.RedPhase;
                }
                else if (collisionLayer == "BluePhaseCrystal")
                {
                    currentPhase = Phases.BluePhase;
                }
                else if (collisionLayer == "GreenPhaseCrystal")
                {
                    currentPhase = Phases.GreenPhase;
                }
                else if (collisionLayer == "YellowPhaseCrystal")
                {
                    currentPhase = Phases.YellowPhase;
                }
                break;

            case Phases.BluePhase:
                if (collisionLayer == "DefaultPhaseCrystal" || collisionLayer == "BluePhaseCrystal" ||
                    collisionLayer == "RedPhaseCrystal" || collisionLayer == "GreenPhaseCrystal" ||
                    collisionLayer == "YellowPhaseCrystal" || collisionLayer == "BlueGround" ||
                    collisionLayer == "BlueWall" || collisionLayer == "BlueCorner" || collisionLayer == "Wall" ||
                    collisionLayer == "Ground" || collisionLayer == "Corner")
                {
                    currentPhase = Phases.BluePhase;
                }
                break;

            case Phases.RedPhase:
                if (collisionLayer == "DefaultPhaseCrystal" || collisionLayer == "BluePhaseCrystal" ||
                    collisionLayer == "RedPhaseCrystal" || collisionLayer == "GreenPhaseCrystal" ||
                    collisionLayer == "YellowPhaseCrystal" || collisionLayer == "RedGround" ||
                    collisionLayer == "RedWall" || collisionLayer == "RedCorner" || collisionLayer == "Wall" ||
                    collisionLayer == "Ground" || collisionLayer == "Corner")
                {
                    currentPhase = Phases.RedPhase;
                }
                break;

            case Phases.GreenPhase:
                if (collisionLayer == "DefaultPhaseCrystal" || collisionLayer == "BluePhaseCrystal" ||
                    collisionLayer == "RedPhaseCrystal" || collisionLayer == "GreenPhaseCrystal" ||
                    collisionLayer == "YellowPhaseCrystal" || collisionLayer == "GreenGround" ||
                    collisionLayer == "GreenWall" || collisionLayer == "GreenCorner" || collisionLayer == "Wall" ||
                    collisionLayer == "Ground" || collisionLayer == "Corner")
                {
                    currentPhase = Phases.GreenPhase;
                }
                break;

            case Phases.YellowPhase:
                if (collisionLayer == "DefaultPhaseCrystal" || collisionLayer == "BluePhaseCrystal" ||
                    collisionLayer == "RedPhaseCrystal" || collisionLayer == "GreenPhaseCrystal" ||
                    collisionLayer == "YellowPhaseCrystal" || collisionLayer == "YellowGround" ||
                    collisionLayer == "YellowWall" || collisionLayer == "YellowCorner" || collisionLayer == "Wall" ||
                    collisionLayer == "Ground" || collisionLayer == "Corner")
                {
                    currentPhase = Phases.YellowPhase;
                }
                break;
        }
    }
}
