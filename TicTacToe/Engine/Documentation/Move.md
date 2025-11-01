```mermaid
flowchart TB
    A((API)) -- Move --> NM(Move: Status := Pending)
    NM -- Insert --> NS(((Session)))
    NS --> GS
    NS --> Move
    NS --> Grid
    NS --> L
    GS[Game Stage] -- "[Stage = Active]" --> Ant{MoveRule}
    Move -- "[Status = Pending]" --> Ant
    Grid -- "[Move cell is empty]" --> Ant
    L["Line(*)"] -- "[Accepts Move]" --> Ant
    Ant --> MS(Move: Status := Processed)
    MS --> GRS(Grid: Fill cell with move)
    GRS --> LS(Lines: Fill cell with move)
    LS --> GD{Draw?}
    LS --> GW{Win?}