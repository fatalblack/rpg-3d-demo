using System;
using System.Collections.Generic;

public static class Calculos
{
    public static EntidadEstadisticaBase CalcularEstadisticasEquipable(ObjetoEstadistica estadisticaObjeto)
    {
        // establecemos las estad�sticas principales base
        int fuerza = estadisticaObjeto.Fuerza;
        int agilidad = estadisticaObjeto.Agilidad;
        int inteligencia = estadisticaObjeto.Inteligencia;
        int vitalidad = estadisticaObjeto.Vitalidad;

        // calculamos las estad�sticas en base a los 4 stats principales (fuerza, agilidad, inteligencia y vitalidad)
        EntidadEstadisticaBase estadisticas = CalcularEstadisticasBase(fuerza, agilidad, inteligencia, vitalidad, 1);

        // retornamos las estad�sticas
        return estadisticas;
    }

    public static EntidadEstadisticaBase CalcularEstadisticasBase(int fuerzaBase, int agilidadBase, int inteligenciaBase, int vitalidadBase, int factorNivel)
    {
        // establecemos las estad�sticas principales base
        int fuerza = fuerzaBase * factorNivel;
        int agilidad = agilidadBase * factorNivel;
        int inteligencia = inteligenciaBase * factorNivel;
        int vitalidad = vitalidadBase * factorNivel;

        // calculamos las estad�sticas en base a los 4 stats principales (fuerza, agilidad, inteligencia y vitalidad)
        EntidadEstadisticaBase estadisticas = new EntidadEstadisticaBase
        {
            Fuerza = fuerza,
            Agilidad = agilidad,
            Inteligencia = inteligencia,
            Vitalidad = vitalidad,
            AtaqueCalculado = CalcularAtaque(fuerza),
            VelocidadAtaqueCalculada = CalcularVelocidadAtaque(agilidad),
            EvasionCalculada = CalcularEvasion(agilidad, inteligencia),
            VidaMaximaCalculada = CalcularVidaMaxima(vitalidad),
            VidaActualCalculada = CalcularVidaMaxima(vitalidad)
        };

        // retornamos las estad�sticas
        return estadisticas;
    }

    public static int CalcularAtaque(int fuerza)
    {
        // calculamos (factor * fuerza)
        int factor = 3;
        int ataque = factor * fuerza;

        // retornamos el ataque
        return ataque;
    }

    public static double CalcularVelocidadAtaque(int agilidad)
    {
        // calculamos (factor * agilidad)
        double factor = 0.01;
        double velocidadAtaque = factor * agilidad;

        // retornamos la velocidad de ataque
        return velocidadAtaque;
    }

    public static double CalcularEvasion(int agilidad, int inteligencia)
    {
        // calculamos (agilidad + (inteligencia / 2)) / factor
        double factor = 10;
        double evasion = (agilidad + (inteligencia / 2)) / factor;

        // retornamos la evasi�n
        return evasion;
    }

    public static int CalcularVidaMaxima(int vitalidad)
    {
        // calculamos (factor * vitalidad)
        int factor = 4;
        int vidaMaxima = factor * vitalidad;

        // retornamos la vida m�xima
        return vidaMaxima;
    }

    public static int CalcularAtaqueMitigado(int fuerza, int vitalidad)
    {
        // calculamos ((fuerza * 0.2) * (vitalidad * 0.6))
        int armadura = (int)Math.Round(fuerza * 0.2 + vitalidad * 0.6, 0);

        // retornamos la armadura
        return armadura;
    }

    public static int CalcularAtaqueFinal(int ataque, int agilidad, int ataqueMitigado)
    {
        // en base a -> Ataque sin mitigar + Ataque mitigado
        // calculamos el porcentaje que le corresponde a -> Ataque sin mitigar
        // el da�o que aplicar�mos ser� Ataque sin mitigar * porcentaje
        double ataqueSinMitigar = ataque * (1 + (double)agilidad / 100);
        double totalesAtaque = ataqueSinMitigar + ataqueMitigado;
        double porcentajeAtaque = ataqueSinMitigar / totalesAtaque;
        int ataqueFinal = (int)Math.Round(ataqueSinMitigar * porcentajeAtaque, 0);

        // retornamos el ataque final
        return ataqueFinal;
    }

    public static bool EsAtaqueEvadido(double evasion)
    {
        // en base al porcentaje de evasi�n generamos la posibilidades de que evada o no
        double evasionPorcentaje = 100 - Math.Round(evasion, 0);
        List<bool> listaPosibilidades = new List<bool>();
        int itemPosibilidadElegido = new Random().Next(0, 99);

        for (int i = 0; i < 100; i++)
        {
            listaPosibilidades.Add(i >= evasionPorcentaje);
        }

        // retornamos si evadi� o no
        return listaPosibilidades[itemPosibilidadElegido];
    }
}