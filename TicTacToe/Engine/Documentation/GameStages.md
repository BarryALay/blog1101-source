```mermaid
stateDiagram
    direction LR
    [*] --> Initial : Initialize
    Initial --> New : API.NewGame
    New --> Active : NewGameRule
    Active --> Error : InvalidMoveRule
    Active --> Error : DuplicateMoveRule
    Error --> Active : NewGame
    Active --> Win : HaveWinRule
    Active --> Draw : HaveDrawRule
