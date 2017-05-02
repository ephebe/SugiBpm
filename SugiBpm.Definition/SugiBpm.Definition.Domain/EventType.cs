namespace SugiBpm.Definition.Domain
{
    public enum EventType
    {
        PROCESS_INSTANCE_START = 1,
        PROCESS_INSTANCE_END = 2,
        PROCESS_INSTANCE_CANCEL = 3,

        FLOW_START = 4,
        FLOW_END = 5,
        FLOW_CANCEL = 6,

        FORK = 7,
        JOIN = 8,
        TRANSITION = 9,
        BEFORE_DECISION = 10,
        AFTER_DECISION = 11,

        BEFORE_ACTIVITYSTATE_ASSIGNMENT = 12,
        AFTER_ACTIVITYSTATE_ASSIGNMENT = 13,
        BEFORE_PERFORM_OF_ACTIVITY = 14,
        PERFORM_OF_ACTIVITY = 15,
        AFTER_PERFORM_OF_ACTIVITY = 16,
        SUB_PROCESS_INSTANCE_START = 17,
        SUB_PROCESS_INSTANCE_COMPLETION = 18,

        ACTION = 19,
        DELEGATION_EXCEPTION = 20
    }
}
