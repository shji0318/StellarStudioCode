<span style="color: red;">아래는 해당 리포지토리의 간략한 설명입니다.</span>

---

## 업적 시스템


<p align = "center"><img src="https://file.notion.so/f/f/102d0a0d-f982-4f89-877e-6d4b07addbc7/d50db2ae-de90-48af-b56f-0de1c6922740/Achievement.gif?id=2e29abd7-57ab-41a8-acb0-c5cefbdde4fe&table=block&spaceId=102d0a0d-f982-4f89-877e-6d4b07addbc7&expirationTimestamp=1704254400000&signature=tGCr3unJnIDwaaKvsX9tk4VNJOv90gbs_OEq1ksBMYo&downloadName=Achievement.gif">
</p>


### UML


<p align = "center"><img src="https://file.notion.so/f/f/102d0a0d-f982-4f89-877e-6d4b07addbc7/41c5b6e2-823b-4c27-bf14-80eb996ce031/AchievementUML.png?id=3707107e-a42b-4a27-aea3-cc8c5888efec&table=block&spaceId=102d0a0d-f982-4f89-877e-6d4b07addbc7&expirationTimestamp=1704254400000&signature=6pTRxXe4c-uT_Wg6jh_eyla3IL17K89wJvfteSsVYI0&downloadName=AchievementUML.png" height = "500px" weight = "100px">
</p>


### 개발과정
- 게임 플레이 시 스테이지 클리어 뿐 아니라 업적 시스템을 통해 **도전 목표를 제공**하기 위해 개발하게 되었습니다.
- **옵저버 패턴을 사용**하여 특정 클래스에서 조건을 만족하였을 경우, 업적 달성을 알리고 AcievementManager에서 해당 항목을 담고 있다 MainScene으로 넘어왔을 때 알람을 활성화 합니다.


### 간단한 클래스 설명
- [**AchievementManager.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/Achievement/AchievementManager.cs)
  - 싱글톤 패턴을 사용
  - 업적 DB와 업적 달성 이벤트를 관리하는 클래스
 
  
- [**Achievement.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/Achievement/Achievement.cs)
  - 업적 ID, Title, 달성 목표 데이터를 담고 있는 클래스
 
  
- [**AchievementSlot.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/Achievement/Achievement_Slot.cs)
  - UI_Base를 상속 받아 Text 및 Image들을 해당 업적 달성 유무에 따라 유동적으로 관리
 
  


---

## 최적화


### 개발과정
- 로그 라이크 게임 특성상 수많은 몬스터가 생성되고 파괴되는데 **오브젝트 풀링**을 사용하여 이를 최적화 했습니다.
- 풀링뿐만 아니라 Unity2D 게임 개발에서 Batches를 줄이는 방법인 Sprite Atlas를 통한 image Packing
- 고정된 데이터 값에 대한 메모리를 효율적으로 사용하도록 ScriptableObject, 게임 빌드시 용량을 최소화하기 위해 AWS서버를 이용하여 Addressable을 사용했습니다.


### 간단한 클래스 설명
- [**PoolingManager.cs**](https://github.com/shji0318/StellarStudioCode/tree/main/Optimization)
  - 오브젝트 풀링을 관리하는 클래스
  - Dictionary 자료구조를 사용했으며 Key값으로 오브젝트의 이름(string) , Value로는 Pooling 클래스를 갖습니다.

  
- [**Pooling.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/Optimization/Pooling.cs)
  - Pooling할 오브젝트를 Stack으로 관리
  - Pool의 단위로 사용
 

- [**Util.cs**](https://github.com/shji0318/StellarStudioCode/tree/main/Optimization)
  - 유니티에서 제공하는 함수를 랩핑을 통해 기능을 추가하여 사용하기 위한 전역 클래스입니다.
  - GameObject.Instantiate를 랩핑하여 Poolable이라는 컴포넌트를 갖은 오브젝트라면 풀링에서 관리하도록하여 사용했습니다.


---

## 플레이어 및 몬스터


<p align = "center"><img src="https://file.notion.so/f/f/102d0a0d-f982-4f89-877e-6d4b07addbc7/d9086ccf-a562-49e9-9a33-3f0cafebeebc/Monster.gif?id=e5d161a4-b0f7-4931-904e-5cc6e0f037c7&table=block&spaceId=102d0a0d-f982-4f89-877e-6d4b07addbc7&expirationTimestamp=1704268800000&signature=SvKZjBZUZmnbYfP8XVQcDy5WPmLu3F6zxQNVTvgjU1w&downloadName=Monster.gif">
</p>


### UML


<p align = "center"><img src="https://file.notion.so/f/f/102d0a0d-f982-4f89-877e-6d4b07addbc7/eda382be-7a9c-4e65-a51e-048e12e8574e/CreatureUML.png?id=d7f4425c-5c1b-4754-af42-3eb94eb8deb6&table=block&spaceId=102d0a0d-f982-4f89-877e-6d4b07addbc7&expirationTimestamp=1704268800000&signature=RBBvXu7mm-p7uLxU6J69knMjufFXVCi0_cHvGRuX1eA&downloadName=CreatureUML.PNG.png" height = "500px" weight = "100px">
</p>


### 개발과정
- 게임의 가장 기본 요소인 플레이어와 몬스터는 아이템과 더불어 추후 개체가 추가될 가능성이 컸기 때문에 최대한 **코드 재사용성을 늘려 개발 효율을 늘리는 것을 목표**로 했습니다. 
- Monster와 Player가 공통으로 사용하는 특성 및 State를 추상 클래스인 CreatureController를 작성한 후, **상속하여 코드의 재사용성을 늘리도록 설계**했습니다.

### 간단한 클래스 설명
- [**CreatureController.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/Achievement/CreatureController.cs)
  - Monster와 Player의 공통된 데이터를 갖는 추상 클래스
  - Monster와 Player 둘다 이동, 공격, 죽음과 같은 공통된 상태를 갖기 때문에 CreatureController에서 상태패턴을 통해 애니메이션 및 행동로직을 구현했습니다.
 
  
- [**MonsterController.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/PlayerAndMonster/MonsterController.cs)
  - CreatureController를 상속 받는 클래스
  - 몬스터들 종류와 상관없는 공통된 기능들을 구현한 후, 몬스터 종류마다 개별적인 능력치는 **ScriptableObject를 통해 몬스터마다 스크립트를 생성하는 것이 아닌 하나의 스크립트로 재사용성을 늘렸습니다.**
    - 팩토리 패턴을 사용하여 구현하는 것과 프리팹으로 결합한 후 리소스 단위로 관리하는 것 중 Addressable을 사용했을 때 후자가 더 편하다 생각하여 이런식으로 구현해 보았습니다.
 
  
- [**MonsterSpawner.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/PlayerAndMonster/MonsterSpawner.cs)
  - 몬스터를 생성하는 로직들을 갖고 있는 클래스
  - 일정 주기마다 몬스터를 스폰하는 로직과 특정 게임플레이 타임때마다 이벤트성으로 등장하는 몬스터들을 스폰하는 로직들로 구성되어 있습니다.
  - 해당 게임이 무한맵 개념이다 보니 고정 위치에 몬스터가 스폰될 경우 부자연스러움을 느낄 수 있다고 판단하여 플레이어 주변에 스폰되도록 하였습니다.
    - 카메라 범위 내에서 몬스터가 스폰될 경우 플레이어 입장에서 즉각적인 대응이 힘들기 때문에 적어도 카메라 범위 밖에서 스폰되어야 한다고 생각했습니다.
    - 이를 실행하기 위해 임의의 벡터를 구하는 공식인 **(노름 * Cos , 노름 * sin)을 이용해서 노름을 카메라의 범위로 두고 몬스터를 스폰**해서 해결했습니다.


- [**PlayerController.cs**](https://github.com/shji0318/StellarStudioCode/blob/main/PlayerAndMonster/PlayerController.cs)
  - CreatureController를 상속 받는 클래스
  - 플레이어의 인풋에 따른 캐릭터의 행동 및 애니메이션을 상태 패턴으로 구현했습니다.
    
 
  


---
