# Ancient & Medieval Japan - Crop Growth Balance — 設計書

**対象:** RimWorld 1.6。バニラ6作物と任意導入のMedieval Overhaul（MO）7作物。実装は `growDays` のXML置換のみ。1.7は未対応・未検証。

## 目的・評価基準（2026-10-15 作者確定）

**AMJ Grainsで採用した穀物のゲーム内生育期間を基準に、バニラ・MOの既存作物の相対的な生育期間を合わせる**ことが目的。CCTOのような温度・枯死・休眠ルールの調整ではない。

- AMJ Grainsが所有する七穀の生育日数は、アワ6・ヒエ6・キビ5・ソバ4・大麦10・小麦12・稲5。Grains内の加工・用途・土地条件を考慮して設定された値を比較の**基準**とする。これらの数値をこのModから変更しない。
- 既存作物13件は、Grains作物との相対的な早晩差が不自然にならないように調整する。現実の成熟期間は参考資料であり、現実の日数を `growDays` に直接変換しない。
- 作物の収穫数、栄養、加工の有無、労働量を一律に均等化しない。現バージョンで `harvestYield`・研究・播種条件・温度・品目・料理・画像を変更しない。将来の収穫量調整は独立の新規仕様決定がある場合だけ検討する。年間カロリー/面積/労働の精密モデルは**初回完成条件から除外**。
- 独立・任意のXML Modとし、Grains・MO・CCTOをハード依存にしない。Grainsなしでも使用できるが、**Grainsの作物バランスに合わせる併用Mod**であることを説明する。MOが有効な場合だけMOパッチをロードする。
- Grainsが七穀、`Plant_Rice`と条件付きMO小麦を所有し、本Modはそれらに一切パッチしない。CCTOは低温・枯死・休眠を所有する。

## 採用 `growDays`（1.6）

| 提供元 | PlantDef | 元の1.6 | 採用値 |
|---|---|---:|---:|
| Vanilla | `Plant_Potato` | 5.8 | **6** |
| Vanilla | `Plant_Corn` | 11.3 | **13** |
| Vanilla | `Plant_Strawberry` | 4.6 | **6** |
| Vanilla | `Plant_Cotton` | 8 | **10** |
| Vanilla | `Plant_Hops` | 5 | **7** |
| Vanilla | `Plant_Smokeleaf` | 7.5 | **9** |
| MO | `DankPyon_Plant_Onions` | 4 | **8** |
| MO | `DankPyon_Plant_Lentils` | 3 | **8** |
| MO | `DankPyon_Plant_Garlic` | 4 | **10** |
| MO | `DankPyon_Plant_Flax` | 7 | **9** |
| MO | `DankPyon_Plant_Sugarcane` | 5.8 | **12** |
| MO | `DankPyon_Plant_Carrots` | 3.25 | **6** |
| MO | `DankPyon_Plant_Tomatoes` | 4.25 | **7** |

`growDays`は暦日ではなく、実際の成熟は気温・日照・土壌によって変わる。13件の相対値について明白な矛盾が見つからなければ現行値を維持し、収穫数等の精密な均衡調整は行わない。

## 実装・互換性

- 正本: `Patches/CropGrowthBalance.xml` と `Compatibility/MedievalOverhaul/Patches/MOCropGrowthBalance.xml`。
- `loadFolders.xml` の1.6ルートと `IfModActive="DankPyon.Medieval.Overhaul"` によりMOパッチを任意適用する。異なるXML相対パスを維持すること。同名にするとMO用オーバーレイがバニラ用パッチを上書きした実績がある。
- Grainsとの同時ロードではGrains七穀の設定を尊重し、バニラ・MO既存作物13件のみを変更する。
- セーブ内の作物Def名、資源、研究やレシピを新設・変更しない。**既存セーブへの追加・削除テストは未実施**のため公開説明では互換性を断定しない。
- 本番にはRimWorld実行時テスト用C#・Quickstart・Pickleを含めない。

## 完成・同時公開の条件（2026-10-15 作者確定）

1. 13件の値がGrains基準に概ね整合し、明白な取り違えがないこと。独自の歴史期間への厳密な換算や収量・栄養・Pawn作業時間の均等化は不要。
2. 1.6バニラ・MO（任意）およびCCTO併用において、期待する13件がロードされ、播種・成長・収穫に**このMod由来の重大なバグが認められない**こと。Grainsとの競合がないこと。既存の個別機能テストを再利用し、不必要な全季節シミュレーション・精密労働解析・長期実プレイを求めない。
3. 配布物、About、README、Workshop/2game原稿の機能・任意依存・対象バージョンが一致すること。実ゲームテスト未実施と記す古い説明は修正すること。
4. **Grainsが公開可能になった時期に、Crop Growth Balanceも同時公開する。単独先行公開はしない。** Grainsの実プレイで独自作物を優先して検証しているため、CGB独自の長期実プレイは公開条件にしない。Steam/2gameへの投稿・可視性変更は、両Modの準備と公開手順が確認できた段階で別途実行する。

## 現時点の1.6実機証拠と未達

原本MO・CCTOを用いた隔離Linux 1.6において、[生育日数の読み込み・実Pawn播種／収穫／生長](Testing/Native16-CropGrowth-20261014.md) と [Grains同時ロード](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38577898742) の個別Pickle検証は全4構成で成功。Grains同時ロードによる値の衝突も検出されていない。

**注意:** MO・MO+CCTOの原本プロバイダーに既知のFMOD音声エラーが各2件あり、既存の厳格Unity ERROR0ワークフロー判定は**2/4、全体FAILのまま**。これはCrop Growth BalanceのXML機能テスト成功と区別し、4/4 ERROR0や包括的ゲーム環境PASSと主張しない。音声不具合は別系統で管理する。1.7対応、セーブ継続性、最終配布ペイロード、公開実行は未確認。詳細な15作物の収量・栄養・Pawn作業・1日tick検証は [既存の参考監査](Balance/PreliminaryHarvestProductivity-20261014.md) に保存し、**完成条件として使用しない**。
