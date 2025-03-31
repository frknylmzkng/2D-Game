using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    // Karakterin hasar alması ve aldığı hasar miktarı
    public static UnityAction<GameObject, int> characterDamaged; //18x15.45

    // Karakterin iyileşmesi ve iyileşme miktarı
    public static UnityAction<GameObject, int> characterHealed; //18x15.45
}

