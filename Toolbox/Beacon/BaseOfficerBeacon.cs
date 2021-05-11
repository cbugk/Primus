using System;
using System.Collections.Generic;
using Primus.Core.Bibliotheca;
using UnityEngine;

namespace Primus.Toolbox.Beacon
{
    public abstract class BaseOfficerBeacon<TCatalogBeacon, TBibliotheca>
        : MonoBehaviour
        where TCatalogBeacon : Enum
        where TBibliotheca : BaseBibliotheca<TCatalogBeacon>
    {
        public TBibliotheca Bibliotheca { get; protected set; }


    }
}
