#include "MonoManager.h"

#ifdef  _WIN32
#include <Windows.h>
#endif //  _WIN32

#include <mono/jit/jit.h>
#include <mono/metadata/assembly.h>
#include <mono/metadata/debug-helpers.h>
#include <mono/metadata/attrdefs.h>

#include <iostream>
#include <string>

namespace MonoManager {

	static MonoDomain* domain = nullptr;
	static MonoImage* image = nullptr;
	static MonoImage* CoreAssemblyImage = nullptr;

	static MonoMethod* GetMethod(MonoImage* image, const std::string& methodDesc);
	struct MonoScript
	{
		std::string FullName;
		std::string ClassName;
		std::string NamespaceName;

		MonoClass* Class = nullptr;
		MonoMethod* OnCreateMethod = nullptr;
		MonoMethod* OnDestroyMethod = nullptr;
		MonoMethod* OnUpdateMethod = nullptr;

		// Physics
		MonoMethod* OnCollision2DBeginMethod = nullptr;
		MonoMethod* OnCollision2DEndMethod = nullptr;

		void InitClassMethods(MonoImage* image)
		{
			OnCreateMethod = GetMethod(image, FullName + ":OnCreate()");
			OnUpdateMethod = GetMethod(image, FullName + ":OnUpdate(single)");

			// Physics (Entity class)
			OnCollision2DBeginMethod = GetMethod(CoreAssemblyImage, "x.Entity:OnCollision2DBegin(single)");
			OnCollision2DEndMethod = GetMethod(CoreAssemblyImage, "x.Entity:OnCollision2DEnd(single)");
		}
	};

	//MonoObject* MonoScriptInstance::GetInstance()
	//{
	//	//HZ_CORE_ASSERT(Handle, "Entity has not been instantiated!");
	//	return mono_gchandle_get_target(Handle);
	//}

	static void Init()
	{
		//.Net dlls
		//mono_set_assemblies_path("mono/lib");
		// mono_jit_set_trace_options("--verbose");
		auto domain = mono_jit_init("EmbeddingMono");
		char* name = (char*)"MonoRuntime";
		domain = mono_domain_create_appdomain(name, nullptr);
		//assembly = mono_domain_assembly_open(domain, "file.exe");
	}

	static void Shutdown()
	{
		mono_jit_cleanup(domain);
	}

	static MonoAssembly* LoadAssemblyFromFile(const char* filepath)
	{
		if (filepath == NULL)
		{
			return NULL;
		}

		HANDLE file = CreateFileA(filepath, FILE_READ_ACCESS, 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
		if (file == INVALID_HANDLE_VALUE)
		{
			return NULL;
		}

		DWORD file_size = GetFileSize(file, NULL);
		if (file_size == INVALID_FILE_SIZE)
		{
			CloseHandle(file);
			return NULL;
		}
		void* file_data = malloc(file_size);
		if (file_data == NULL)
		{
			CloseHandle(file);
			return NULL;
		}

		DWORD read = 0;
		ReadFile(file, file_data, file_size, &read, NULL);
		if (file_size != read)
		{
			free(file_data);
			CloseHandle(file);
			return NULL;
		}

		MonoImageOpenStatus status;
		MonoImage* image = mono_image_open_from_data_full(reinterpret_cast<char*>(file_data), file_size, 1, &status, 0);
		if (status != MONO_IMAGE_OK)
		{
			return NULL;
		}
		auto assemb = mono_assembly_load_from_full(image, filepath, &status, 0);
		free(file_data);
		CloseHandle(file);
		mono_image_close(image);
		return assemb;
	}

	static MonoAssembly* LoadAssembly(const std::string& path)
	{
		MonoAssembly* assembly = LoadAssemblyFromFile(path.c_str());

		if (!assembly)
			std::cout << "Could not load assembly: " << path << std::endl;
		else
			std::cout << "Successfully loaded assembly: " << path << std::endl;

		return assembly;
	}

	static MonoImage* GetAssemblyImage(MonoAssembly* assembly)
	{
		MonoImage* image = mono_assembly_get_image(assembly);
		if (!image)
			std::cout << "mono_assembly_get_image failed" << std::endl;

		return image;
	}

	static MonoClass* GetClass(MonoImage* image, const MonoScript& scriptClass)
	{
		MonoClass* monoClass = mono_class_from_name(image, scriptClass.NamespaceName.c_str(), scriptClass.ClassName.c_str());
		if (!monoClass)
			std::cout << "mono_class_from_name failed" << std::endl;

		return monoClass;
	}

	static uint32_t Instantiate(MonoScript& scriptClass)
	{
		MonoObject* instance = mono_object_new(domain, scriptClass.Class);
		if (!instance)
			std::cout << "mono_object_new failed" << std::endl;

		mono_runtime_object_init(instance);
		uint32_t handle = mono_gchandle_new(instance, false);
		return handle;
	}

	static MonoMethod* GetMethod(MonoImage* image, const std::string& methodDesc)
	{
		MonoMethodDesc* desc = mono_method_desc_new(methodDesc.c_str(), NULL);
		if (!desc)
			std::cout << "mono_method_desc_new failed" << std::endl;

		MonoMethod* method = mono_method_desc_search_in_image(desc, image);
		if (!method)
			std::cout << "mono_method_desc_search_in_image failed" << std::endl;

		return method;
	}

	static MonoObject* CallMethod(MonoObject* object, MonoMethod* method, void** params = nullptr)
	{
		MonoObject* pException = NULL;
		MonoObject* result = mono_runtime_invoke(method, object, params, &pException);
		return result;
	}
}