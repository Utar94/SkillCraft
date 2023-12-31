﻿namespace SkillCraft.Core.Logging
{
  /// <summary>
  /// Le niveau de sévérité d'une erreur. La valeur entière est très importante puisqu'elle est
  /// utilisée afin de déterminer la priorité de la sévérité d'une erreur. Il faut donc conserver
  /// cette priorité si on ajoute des niveaux plus ou moins prioritaires aux niveaux existants, ou
  /// si on insère un nouveau niveau entre deux niveaux existants.
  /// </summary>
  public enum ErrorSeverity
  {
    /// <summary>
    /// Errors that highlight an abnormal or unexpected event in the application flow, but do not
    /// otherwise cause the application execution to stop.
    /// </summary>
    Warning = 0,
    /// <summary>
    /// Errors that highlight when the current flow of execution is stopped due to a failure.
    /// These should indicate a failure in the current activity, not an application-wide failure.
    /// </summary>
    Failure = 1,
    /// <summary>
    /// Errors that describe an unrecoverable application or system crash, or a catastrophic
    /// failure that requires immediate attention.
    /// </summary>
    Critical = 2
  }
}
