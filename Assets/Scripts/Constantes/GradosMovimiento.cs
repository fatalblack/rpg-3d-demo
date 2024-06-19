public static class GradosMovimiento
{
    public const float GradosArriba = 0;

    public const float GradosArribaDerecha = 45;

    public const float GradosDerecha = 90;

    public const float GradosAbajoDerecha = 135;

    public const float GradosAbajo = 180;

    public const float GradosAbajoIzquierda = 225;

    public const float GradosIzquierda = 270;

    public const float GradosArribaIzquierda = 315;

    public static float ObtenerGradoOpuesto(float grado)
	{
		switch (grado)
		{
			case GradosMovimiento.GradosArriba:
				return GradosMovimiento.GradosAbajo;
			case GradosMovimiento.GradosArribaDerecha:
				return GradosMovimiento.GradosAbajoIzquierda;
			case GradosMovimiento.GradosDerecha:
				return GradosMovimiento.GradosIzquierda;
			case GradosMovimiento.GradosAbajoDerecha:
				return GradosMovimiento.GradosArribaIzquierda;
			case GradosMovimiento.GradosAbajo:
				return GradosMovimiento.GradosArriba;
			case GradosMovimiento.GradosAbajoIzquierda:
				return GradosMovimiento.GradosArribaDerecha;
			case GradosMovimiento.GradosIzquierda:
				return GradosMovimiento.GradosDerecha;
			case GradosMovimiento.GradosArribaIzquierda:
				return GradosMovimiento.GradosAbajoDerecha;
		}

		return 0f;
	}
}
