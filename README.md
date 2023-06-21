## GunsAndRachels_Portfolio

![header](https://capsule-render.vercel.app/api?type=rounded&color=auto&height=300&section=header&text=Hong's%20Portfolio&fontSize=90)

# 조작

**조이스틱**

<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/2aa0be08-fa9a-4e14-9ab4-4f59fef79f1f" width="50%" height="50%">

*이동*

터치한 지점에서 조이스틱이 나타나며 그 위치를 기준으로 캐릭터를 이동시킵니다.



<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/2dfe97f6-ae6b-4b7e-980a-91619f164a10" width="50%" height="50%">

*공격*

조이스틱의 중앙을 기반으로 터치된 위치를 계산하여 캐릭터와 총을 회전시키고 조이스틱이 터치되는 동안 해당 방향으로 탄환을 발사합니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/244ee2ba-b795-4f7e-9317-deab23fa2857" width="50%" height="50%">

*스킬*

각각 스킬 버튼은 타입을 가지며 해당 타입에 맞도록 작동합니다. 스킬 상단 빗금표시에서 터치를 해제하면 스킬을 취소합니다.



# 전투

<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/0912fd66-ae4d-4c93-bf74-187c03a5e22b" width="50%" height="50%">

*총기 변경*

게임 내의 아이템을 획득하여 기본 총기를 변경할 수 있습니다. 이 때 총기에 따라 공격속도, 공격력 공식이 다르게 적용됩니다.


# 스킬


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/212f7f45-aa89-4f77-b6bf-fe49e0b49670" width="50%" height="50%">

*돌격소총 스킬 1*

플레이어를 중심으로 직선형 Skill Indicator를 활성화하여 해당 방향으로 진행하는 탄환형 스킬입니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/af5e9839-33c4-4a07-9daa-0951cf2ea649" width="50%" height="50%">

*돌격소총 스킬 2*

버튼을 터치하여 즉시 사용되며 일정 시간동안 공격력을 증가시키는 버프형 스킬입니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/90ffbd81-7b3b-4c0e-ba65-a5a56929d855" width="50%" height="50%">

*돌격소총 스킬 3*

플레이어를 중심으로 근거리 Skill Indicator를 활성화하여 캐릭터가 바라보는 방향으로 스킬이 생성되어 적을 타격합니다. 총 3회 타격하며 중복 타격 하지 않기 위해 HashSet을 사용합니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/deaa660b-d0e5-453b-a874-25b70d131ab3" width="50%" height="50%">

*저격소총 스킬 1*

플레이어를 중심으로 직선형 Skill Indicator를 활성화하여 해당 방향으로 진행하는 탄환형 스킬입니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/3c3e8106-690d-4489-8b84-b98d3244267e" width="50%" height="50%">

*저격소총 스킬 2*

플레이어를 중심으로 직선형 Skill Indicator를 활성화하여 해당 방향으로 스킬이 생성되며 Physics2D를 사용하여 특정 범위에 들어온 적을 타격합니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/cf7c102a-28d4-473b-a037-7cab42fcac22" width="50%" height="50%">

*저격소총 스킬 3*

플레이어를 중심으로 원형 Skill Indicator를 활성화하여 지정된 위치에 스킬을 생성하여 해당 범위의 적에게 피해를 입힙니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/81c3ef41-4ca6-459e-829f-539f58cbd073" width="50%" height="50%">

*샷건 스킬 1*

플레이어를 중심으로 직선형 Skill Indicator를 활성화하여 해당 방향으로 진행하는 탄환형 스킬입니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/c52aa465-b35f-4452-a726-097355c064ab" width="50%" height="50%">

*샷건 스킬 2*

버튼을 터치하여 즉시 사용되며 일정 시간동안 공격속도를 증가시키는 버프형 스킬입니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/ef1c6b73-ab90-46df-87a9-421bdfddf70f" width="50%" height="50%">

*샷건 스킬 3*

버튼을 터치하여 플레이어를 중심으로 적에게 피해를 주는 스킬을 생성합니다. 몬스터의 bool타입 변수값을 활용하여 특정 시간동안 중복 데미지를 입지 안도록합니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/29ddfaa1-a66a-41e7-9759-6291197fe204" width="50%" height="50%">

*기관단총 스킬 1*

플레이어를 중심으로 직선형 Skill Indicator를 활성화하여 해당 방향으로 진행하는 탄환형 스킬입니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/1f544bf3-f15a-463f-9b2a-b8282923c219" width="50%" height="50%">

*기관단총 스킬 2*

버튼을 터치하여 즉시 사용되며 일정 시간동안 치명타확률을 증가시키는 버프형 스킬입니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/944e13f5-8ad3-4682-8cfd-969ae843f1d6" width="50%" height="50%">

*기관단총 스킬 3*

플레이어를 중심으로 원형 Skill Indicator를 활성화하여 지정된 위치에 스킬을 생성하여 해당 범위의 적에게 피해를 입힙니다.




# 총기 특수효과 (고대유물)


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/da17b4ab-7e5b-473b-ab78-e48e68e9325f" width="50%" height="50%">

*레이저 사이트*

기본 공격의 탄환 방향으로 LineRenderer를 사용하여 레이저 효과를 만듭니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/e17fb25a-50b0-4c56-90e7-cea2b24bd328" width="50%" height="50%">

*대시 공격*

플레이어가 대시하는 동안 적과 충돌하면 적에게 피해를 입힙니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/70c9a5fc-4723-4903-9b6b-0051881cb6f5" width="50%" height="50%">

*방어 탄환*

플레이어의 기본 공격 탄환이 적 탄환과 충돌하여 파괴합니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/3372d74c-ca56-4efe-8175-94f35cc8acc0" width="50%" height="50%">

*독 탄환*

플레이어의 기본 공격 탄환이 적에게 적중할 시 중독효과를 만들어 지속적인 피해를 입힙니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/94d64894-da96-49b4-b19b-4d6bb871358d" width="50%" height="50%">

*유탄발사기*

플레이어가 기본 공격을 하는 동안 유탄을 추가로 발사하여 적에게 피해를 입힙니다.



# 몬스터

<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/e3095872-e3b0-4582-a9be-e9380dcf1d8f" width="50%" height="50%">

*길찾기*

모든 몬스터들은 A* 알고리즘을 활용하여 장애물을 피해 길찾기를 하며 이동합니다. 


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/fffd50f6-ea7e-4c1d-b8c2-eec7c177a052" width="50%" height="50%">
<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/9546ab6d-4d5c-41e7-b3b4-d7398074cbe1" width="50%" height="50%">
<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/fe06eb99-f356-4991-8f3e-8a51f282a1dc" width="50%" height="50%">

*근접 몬스터*

근접 몬스터는 플레이어와의 거리를 계산하여 특정 거리 이하에서는 이동속도가 빨라지며 공격 애니메이션이 실행됩니다. 


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/db0dac15-f52f-4be5-a8a9-23643e91af0e" width="50%" height="50%">
<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/d9b67829-3d38-4fb3-a445-96da9508ff7e" width="50%" height="50%">

*대시 몬스터*

대시 몬스터는 플레이어와의 거리를 계산하여 특정 거리 이하에서는 대시공격을 합니다. 공격 시 slash 이펙트 범위에 trigger collider를 생성하여 공격합니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/75b03fb5-35bd-4c69-8f65-37cb5fa18458" width="50%" height="50%">
<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/9596ca4e-2696-4e30-8afb-08a9770fd06d" width="50%" height="50%">

*레이지 몬스터*

레이지 몬스터는 랜덤한 시간마다 분노상태가 되어 이동속도가 빨라지고 넉백을 무시합니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/3a57d38e-5bbf-478a-b63a-e94bf73dd681" width="50%" height="50%">
<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/62e374bb-25d5-4504-9105-cf0936f94e31" width="50%" height="50%">
<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/cc71c28a-6851-405d-b095-9be3d6c2d250" width="50%" height="50%">

*원거리 몬스터*

원거리 몬스터는 플레이어와의 거리를 계산하여 특정 거리 이하에서는 탄환을 발사합니다. 특정 몬스터는 2가지 패턴의 원거리 공격을 합니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/30fbdced-d2fc-4028-9d33-6255d080dbed" width="50%" height="50%">
<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/a774a922-b941-4e30-86c0-907ec3a04d5f" width="50%" height="50%">
<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/8748ff1e-227e-441f-88ce-f61e97e0c9ac" width="50%" height="50%">

*멀티샷 몬스터*

멀티샷 몬스터는 플레이어와의 거리를 계산하여 특정 거리 이하에서는 여러 갈래의 탄환을 발사합니다.


<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/1bb0bb80-9d16-4533-a9d9-b736f919e262" width="50%" height="50%">
<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/b8e079bc-0864-4918-ae7d-db5bc8d1ca6b" width="50%" height="50%">
<img src = "https://github.com/HSH12345/GunsAndRachels_Portfolio/assets/124248037/3b0bcb12-6a22-4efe-9c26-dd429c5cf140" width="50%" height="50%">

*자폭 몬스터*

자폭 몬스터는 플레이어와의 거리를 계산하여 특정 거리 이하에서 플레이어에게 대시하여 자폭합니다.



[구글 플레이 바로가기](https://play.google.com/store/apps/details?id=com.teamvizeon.gunsandrachels&hl=ko&gl=KR)
