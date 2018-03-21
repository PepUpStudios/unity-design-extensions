
namespace State {
    public interface IState {

        string GetName();

        void EnableState(bool flag);

        void Init(); // Gets called once on game start

        void PreEntryNewTransition(IState newState);

        void PostEntryNewTransition(IState newState);

        void PreTransition(IState previousState);

        void PostTransition(IState previousState);

        void PreTransition<T>(IState previousState, params T[] objs);

        void PostTransition<T>(IState previousState, params T[] objs);

        bool IsTransitionAllowed(IState previousState);

    }
}