#pragma once

extern "C" {
	typedef struct _MonoObject MonoObject;
}

namespace MonoManager
{
	struct MonoScript;
	struct MonoScriptInstance
	{
		MonoScript* script = nullptr;

		//uint32_t Handle = 0;
		//Scene* SceneInstance = nullptr;

		//MonoObject* GetInstance();
	};
}