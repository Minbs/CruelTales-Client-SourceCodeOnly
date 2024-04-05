#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
public class SceneJumperExtension : EditorWindow
{


	[MenuItem("Jumper/Art/scn_workbench_art_level_dowoksini", priority = 100)]
	private static void ChangeTo_Jumper_Art_scn_workbench_art_level_dowoksini()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Art/scn_workbench_art_level_dowoksini.unity");
	}

	[MenuItem("Jumper/Art/scn_workbench_art_level_Horus", priority = 100)]
	private static void ChangeTo_Jumper_Art_scn_workbench_art_level_Horus()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Art/scn_workbench_art_level_Horus.unity");
	}

	[MenuItem("Jumper/Art/scn_workbench_art_level_redhood", priority = 100)]
	private static void ChangeTo_Jumper_Art_scn_workbench_art_level_redhood()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Art/scn_workbench_art_level_redhood.unity");
	}

	[MenuItem("Jumper/Design/sen_workbench_design_level_bremen", priority = 100)]
	private static void ChangeTo_Jumper_Design_sen_workbench_design_level_bremen()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Design/sen_workbench_design_level_bremen.unity");
	}

	[MenuItem("Jumper/Design/sen_workbench_design_level_dowoksini", priority = 100)]
	private static void ChangeTo_Jumper_Design_sen_workbench_design_level_dowoksini()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Design/sen_workbench_design_level_dowoksini.unity");
	}

	[MenuItem("Jumper/Design/sen_workbench_design_level_horus", priority = 100)]
	private static void ChangeTo_Jumper_Design_sen_workbench_design_level_horus()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Design/sen_workbench_design_level_horus.unity");
	}

	[MenuItem("Jumper/Editor/scn_workbench", priority = 100)]
	private static void ChangeTo_Jumper_Editor_scn_workbench()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Editor/scn_workbench.unity");
	}

	[MenuItem("Jumper/Editor/scn_workbench_client_graphics", priority = 100)]
	private static void ChangeTo_Jumper_Editor_scn_workbench_client_graphics()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Editor/scn_workbench_client_graphics.unity");
	}

	[MenuItem("Jumper/Editor/scn_workbench_client_gui", priority = 100)]
	private static void ChangeTo_Jumper_Editor_scn_workbench_client_gui()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Editor/scn_workbench_client_gui.unity");
	}

	[MenuItem("Jumper/Editor/scn_workbench_server", priority = 100)]
	private static void ChangeTo_Jumper_Editor_scn_workbench_server()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Editor/scn_workbench_server.unity");
	}

	[MenuItem("Jumper/Editor/scn_workbench_server_physics", priority = 100)]
	private static void ChangeTo_Jumper_Editor_scn_workbench_server_physics()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Editor/scn_workbench_server_physics.unity");
	}

	[MenuItem("Jumper/Game/scn_error", priority = 100)]
	private static void ChangeTo_Jumper_Game_scn_error()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Game/scn_error.unity");
	}

	[MenuItem("Jumper/Game/scn_game_gameplay", priority = 100)]
	private static void ChangeTo_Jumper_Game_scn_game_gameplay()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Game/scn_game_gameplay.unity");
	}

	[MenuItem("Jumper/Game/scn_game_initial", priority = 100)]
	private static void ChangeTo_Jumper_Game_scn_game_initial()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Game/scn_game_initial.unity");
	}

	[MenuItem("Jumper/Game/scn_game_loader", priority = 100)]
	private static void ChangeTo_Jumper_Game_scn_game_loader()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Game/scn_game_loader.unity");
	}

	[MenuItem("Jumper/Game/scn_game_main_menu", priority = 100)]
	private static void ChangeTo_Jumper_Game_scn_game_main_menu()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Game/scn_game_main_menu.unity");
	}

	[MenuItem("Jumper/Game/scn_game_test_develop", priority = 100)]
	private static void ChangeTo_Jumper_Game_scn_game_test_develop()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Game/scn_game_test_develop.unity");
	}

	[MenuItem("Jumper/GUI/scn_art_ui", priority = 100)]
	private static void ChangeTo_Jumper_GUI_scn_art_ui()
	{
		SceneJumper.ChangeScene("Assets/Scenes/GUI/scn_art_ui.unity");
	}

	[MenuItem("Jumper/Test/scn_test_art_animation", priority = 100)]
	private static void ChangeTo_Jumper_Test_scn_test_art_animation()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/scn_test_art_animation.unity");
	}

	[MenuItem("Jumper/Debug/Input/GamepadVisualizer", priority = 100)]
	private static void ChangeTo_Jumper_Debug_Input_GamepadVisualizer()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Debug/Input/GamepadVisualizer.unity");
	}

	[MenuItem("Jumper/Debug/Input/MouseVisualizer", priority = 100)]
	private static void ChangeTo_Jumper_Debug_Input_MouseVisualizer()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Debug/Input/MouseVisualizer.unity");
	}

	[MenuItem("Jumper/Debug/Input/PenVisualizer", priority = 100)]
	private static void ChangeTo_Jumper_Debug_Input_PenVisualizer()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Debug/Input/PenVisualizer.unity");
	}

	[MenuItem("Jumper/Debug/Input/SimpleControlsVisualizer", priority = 100)]
	private static void ChangeTo_Jumper_Debug_Input_SimpleControlsVisualizer()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Debug/Input/SimpleControlsVisualizer.unity");
	}

	[MenuItem("Jumper/Legacy/Game/legacy_scn_game_ingame", priority = 100)]
	private static void ChangeTo_Jumper_Legacy_Game_legacy_scn_game_ingame()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Legacy/Game/legacy_scn_game_ingame.unity");
	}

	[MenuItem("Jumper/Legacy/Game/legacy_scn_game_initial", priority = 100)]
	private static void ChangeTo_Jumper_Legacy_Game_legacy_scn_game_initial()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Legacy/Game/legacy_scn_game_initial.unity");
	}

	[MenuItem("Jumper/Legacy/Game/legacy_scn_game_title", priority = 100)]
	private static void ChangeTo_Jumper_Legacy_Game_legacy_scn_game_title()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Legacy/Game/legacy_scn_game_title.unity");
	}

	[MenuItem("Jumper/Test/ExecutionTimelineTest/ExecutionTimelineTest_Dowok_Count_1", priority = 100)]
	private static void ChangeTo_Jumper_Test_ExecutionTimelineTest_ExecutionTimelineTest_Dowok_Count_1()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_Dowok_Count_1.unity");
	}

	[MenuItem("Jumper/Test/ExecutionTimelineTest/ExecutionTimelineTest_Dowok_Count_2", priority = 100)]
	private static void ChangeTo_Jumper_Test_ExecutionTimelineTest_ExecutionTimelineTest_Dowok_Count_2()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_Dowok_Count_2.unity");
	}

	[MenuItem("Jumper/Test/ExecutionTimelineTest/ExecutionTimelineTest_Dowok_Film", priority = 100)]
	private static void ChangeTo_Jumper_Test_ExecutionTimelineTest_ExecutionTimelineTest_Dowok_Film()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_Dowok_Film.unity");
	}

	[MenuItem("Jumper/Test/ExecutionTimelineTest/ExecutionTimelineTest_RedHood_Count_1", priority = 100)]
	private static void ChangeTo_Jumper_Test_ExecutionTimelineTest_ExecutionTimelineTest_RedHood_Count_1()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_RedHood_Count_1.unity");
	}

	[MenuItem("Jumper/Test/ExecutionTimelineTest/ExecutionTimelineTest_RedHood_Count_2", priority = 100)]
	private static void ChangeTo_Jumper_Test_ExecutionTimelineTest_ExecutionTimelineTest_RedHood_Count_2()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_RedHood_Count_2.unity");
	}

	[MenuItem("Jumper/Test/ExecutionTimelineTest/ExecutionTimelineTest_RedHood_Film", priority = 100)]
	private static void ChangeTo_Jumper_Test_ExecutionTimelineTest_ExecutionTimelineTest_RedHood_Film()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_RedHood_Film.unity");
	}

	[MenuItem("Jumper/Test/Level/scn_test_level_exe", priority = 100)]
	private static void ChangeTo_Jumper_Test_Level_scn_test_level_exe()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/Level/scn_test_level_exe.unity");
	}

	[MenuItem("Jumper/Test/Level/scn_test_level_sound", priority = 100)]
	private static void ChangeTo_Jumper_Test_Level_scn_test_level_sound()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/Level/scn_test_level_sound.unity");
	}

	[MenuItem("Jumper/Legacy/Game/Gameplay/legacy_scn_gameplay_lobby", priority = 100)]
	private static void ChangeTo_Jumper_Legacy_Game_Gameplay_legacy_scn_gameplay_lobby()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Legacy/Game/Gameplay/legacy_scn_gameplay_lobby.unity");
	}

	[MenuItem("Jumper/Legacy/Game/Gameplay/legacy_scn_gameplay_test", priority = 100)]
	private static void ChangeTo_Jumper_Legacy_Game_Gameplay_legacy_scn_gameplay_test()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Legacy/Game/Gameplay/legacy_scn_gameplay_test.unity");
	}

	[MenuItem("Jumper/Scenes/Art/scn_workbench_art_level_dowoksini", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Art_scn_workbench_art_level_dowoksini()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Art/scn_workbench_art_level_dowoksini.unity");
	}

	[MenuItem("Jumper/Scenes/Art/scn_workbench_art_level_Horus", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Art_scn_workbench_art_level_Horus()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Art/scn_workbench_art_level_Horus.unity");
	}

	[MenuItem("Jumper/Scenes/Art/scn_workbench_art_level_redhood", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Art_scn_workbench_art_level_redhood()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Art/scn_workbench_art_level_redhood.unity");
	}

	[MenuItem("Jumper/Scenes/Design/sen_workbench_design_level_bremen", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Design_sen_workbench_design_level_bremen()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Design/sen_workbench_design_level_bremen.unity");
	}

	[MenuItem("Jumper/Scenes/Design/sen_workbench_design_level_dowoksini", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Design_sen_workbench_design_level_dowoksini()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Design/sen_workbench_design_level_dowoksini.unity");
	}

	[MenuItem("Jumper/Scenes/Design/sen_workbench_design_level_horus", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Design_sen_workbench_design_level_horus()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Design/sen_workbench_design_level_horus.unity");
	}

	[MenuItem("Jumper/Scenes/Editor/scn_workbench", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Editor_scn_workbench()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Editor/scn_workbench.unity");
	}

	[MenuItem("Jumper/Scenes/Editor/scn_workbench_client_graphics", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Editor_scn_workbench_client_graphics()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Editor/scn_workbench_client_graphics.unity");
	}

	[MenuItem("Jumper/Scenes/Editor/scn_workbench_client_gui", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Editor_scn_workbench_client_gui()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Editor/scn_workbench_client_gui.unity");
	}

	[MenuItem("Jumper/Scenes/Editor/scn_workbench_server", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Editor_scn_workbench_server()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Editor/scn_workbench_server.unity");
	}

	[MenuItem("Jumper/Scenes/Editor/scn_workbench_server_physics", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Editor_scn_workbench_server_physics()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Editor/scn_workbench_server_physics.unity");
	}

	[MenuItem("Jumper/Scenes/Game/scn_error", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Game_scn_error()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Game/scn_error.unity");
	}

	[MenuItem("Jumper/Scenes/Game/scn_game_gameplay", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Game_scn_game_gameplay()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Game/scn_game_gameplay.unity");
	}

	[MenuItem("Jumper/Scenes/Game/scn_game_initial", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Game_scn_game_initial()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Game/scn_game_initial.unity");
	}

	[MenuItem("Jumper/Scenes/Game/scn_game_loader", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Game_scn_game_loader()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Game/scn_game_loader.unity");
	}

	[MenuItem("Jumper/Scenes/Game/scn_game_main_menu", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Game_scn_game_main_menu()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Game/scn_game_main_menu.unity");
	}

	[MenuItem("Jumper/Scenes/Game/scn_game_test_develop", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Game_scn_game_test_develop()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Game/scn_game_test_develop.unity");
	}

	[MenuItem("Jumper/Scenes/GUI/scn_art_ui", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_GUI_scn_art_ui()
	{
		SceneJumper.ChangeScene("Assets/Scenes/GUI/scn_art_ui.unity");
	}

	[MenuItem("Jumper/Scenes/Test/scn_test_art_animation", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Test_scn_test_art_animation()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/scn_test_art_animation.unity");
	}

	[MenuItem("Jumper/TestGroup/Deadreckoning/scn_test_deadreckoing", priority = 200)]
	private static void ChangeTo_Jumper_TestGroup_Deadreckoning_scn_test_deadreckoing()
	{
		SceneJumper.ChangeScene("Assets/TestGroup/Deadreckoning/scn_test_deadreckoing.unity");
	}

	[MenuItem("Jumper/Scenes/Debug/Input/GamepadVisualizer", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Debug_Input_GamepadVisualizer()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Debug/Input/GamepadVisualizer.unity");
	}

	[MenuItem("Jumper/Scenes/Debug/Input/MouseVisualizer", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Debug_Input_MouseVisualizer()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Debug/Input/MouseVisualizer.unity");
	}

	[MenuItem("Jumper/Scenes/Debug/Input/PenVisualizer", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Debug_Input_PenVisualizer()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Debug/Input/PenVisualizer.unity");
	}

	[MenuItem("Jumper/Scenes/Debug/Input/SimpleControlsVisualizer", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Debug_Input_SimpleControlsVisualizer()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Debug/Input/SimpleControlsVisualizer.unity");
	}

	[MenuItem("Jumper/Scenes/Legacy/Game/legacy_scn_game_ingame", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Legacy_Game_legacy_scn_game_ingame()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Legacy/Game/legacy_scn_game_ingame.unity");
	}

	[MenuItem("Jumper/Scenes/Legacy/Game/legacy_scn_game_initial", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Legacy_Game_legacy_scn_game_initial()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Legacy/Game/legacy_scn_game_initial.unity");
	}

	[MenuItem("Jumper/Scenes/Legacy/Game/legacy_scn_game_title", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Legacy_Game_legacy_scn_game_title()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Legacy/Game/legacy_scn_game_title.unity");
	}

	[MenuItem("Jumper/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_Dowok_Count_1", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Test_ExecutionTimelineTest_ExecutionTimelineTest_Dowok_Count_1()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_Dowok_Count_1.unity");
	}

	[MenuItem("Jumper/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_Dowok_Count_2", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Test_ExecutionTimelineTest_ExecutionTimelineTest_Dowok_Count_2()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_Dowok_Count_2.unity");
	}

	[MenuItem("Jumper/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_Dowok_Film", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Test_ExecutionTimelineTest_ExecutionTimelineTest_Dowok_Film()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_Dowok_Film.unity");
	}

	[MenuItem("Jumper/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_RedHood_Count_1", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Test_ExecutionTimelineTest_ExecutionTimelineTest_RedHood_Count_1()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_RedHood_Count_1.unity");
	}

	[MenuItem("Jumper/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_RedHood_Count_2", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Test_ExecutionTimelineTest_ExecutionTimelineTest_RedHood_Count_2()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_RedHood_Count_2.unity");
	}

	[MenuItem("Jumper/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_RedHood_Film", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Test_ExecutionTimelineTest_ExecutionTimelineTest_RedHood_Film()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/ExecutionTimelineTest/ExecutionTimelineTest_RedHood_Film.unity");
	}

	[MenuItem("Jumper/Scenes/Test/Level/scn_test_level_exe", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Test_Level_scn_test_level_exe()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/Level/scn_test_level_exe.unity");
	}

	[MenuItem("Jumper/Scenes/Test/Level/scn_test_level_sound", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Test_Level_scn_test_level_sound()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Test/Level/scn_test_level_sound.unity");
	}

	[MenuItem("Jumper/Plugins/Unity-Logs-Viewer/Reporter/Test/ReporterScene", priority = 200)]
	private static void ChangeTo_Jumper_Plugins_Unity_Logs_Viewer_Reporter_Test_ReporterScene()
	{
		SceneJumper.ChangeScene("Assets/Plugins/Unity-Logs-Viewer/Reporter/Test/ReporterScene.unity");
	}

	[MenuItem("Jumper/Plugins/Unity-Logs-Viewer/Reporter/Test/test1", priority = 200)]
	private static void ChangeTo_Jumper_Plugins_Unity_Logs_Viewer_Reporter_Test_test1()
	{
		SceneJumper.ChangeScene("Assets/Plugins/Unity-Logs-Viewer/Reporter/Test/test1.unity");
	}

	[MenuItem("Jumper/Plugins/Unity-Logs-Viewer/Reporter/Test/test2", priority = 200)]
	private static void ChangeTo_Jumper_Plugins_Unity_Logs_Viewer_Reporter_Test_test2()
	{
		SceneJumper.ChangeScene("Assets/Plugins/Unity-Logs-Viewer/Reporter/Test/test2.unity");
	}

	[MenuItem("Jumper/Scenes/Legacy/Game/Gameplay/legacy_scn_gameplay_lobby", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Legacy_Game_Gameplay_legacy_scn_gameplay_lobby()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Legacy/Game/Gameplay/legacy_scn_gameplay_lobby.unity");
	}

	[MenuItem("Jumper/Scenes/Legacy/Game/Gameplay/legacy_scn_gameplay_test", priority = 200)]
	private static void ChangeTo_Jumper_Scenes_Legacy_Game_Gameplay_legacy_scn_gameplay_test()
	{
		SceneJumper.ChangeScene("Assets/Scenes/Legacy/Game/Gameplay/legacy_scn_gameplay_test.unity");
	}

}

#endif