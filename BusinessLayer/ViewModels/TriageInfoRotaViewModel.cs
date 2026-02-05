using DataAccessLayer.Models;
using Enums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ViewModels;

public class TriageInfoRotasViewModel
{
    public TriageInfoRotasViewModel() { }

    public TriageInfoRotasViewModel(IEnumerable<TriageInfoRota> rotas)
    {
        Rotas = new();
        foreach (TriageInfoRota rota in rotas)
        {
            Rotas.Add(new(rota));
        }
    }

    public TriageInfoRotasViewModel(IEnumerable<TriageInfoRotaViewModel> rotas)
    {
        Rotas = new();
        foreach (TriageInfoRotaViewModel rota in rotas)
        {
            Rotas.Add(rota);
        }
    }

    public List<TriageInfoRotaViewModel> Rotas { get; set; }
}


public class TriageInfoRotaViewModel
{
    public TriageInfoRotaViewModel() { }

    public TriageInfoRotaViewModel(TriageInfoRota triageInfoRota)
    {
        TriageInfoRotaId = triageInfoRota.TriageInfoRotaId;
        Day = triageInfoRota.Day;
        Morning = triageInfoRota.Morning;
        Afternoon = triageInfoRota.Afternoon;
        Reserve = triageInfoRota.Reserve;
    }

    public int TriageInfoRotaId { get; set; }
    public WeekdayEnum Day { get; set; }
    public string Morning { get; set; }
    public string Afternoon { get; set; }
    public string Reserve { get; set; }
}
