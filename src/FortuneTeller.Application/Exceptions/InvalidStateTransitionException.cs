namespace FortuneTeller.Application.Exceptions;

public class InvalidStateTransitionException(string message) : Exception(message);
