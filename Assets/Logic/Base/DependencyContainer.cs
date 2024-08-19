using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DependencyContainer : MonoBehaviour
{
    public DependencyDesc[]                  serialisedDependencies;
    protected Dictionary<Type, LogicBase>    dependencies        = new Dictionary<System.Type, LogicBase>();
    protected bool                           initialised         = false;

    [System.Serializable]
    public class DependencyDesc
    {
        public string       comment;
        public string       typeName;
        public LogicBase    dependency;
    }

    // *****************************
    // Init
    // *****************************
    public void Init()
    {
        // check initialized // 
        bool alreadyInitialised = initialised;
        if (alreadyInitialised)
        {
            Debug.LogWarning($"Dependency container { gameObject.name } was already initialised!");
            return;
        }

        // add from descs // 
        for (int i = 0; i < serialisedDependencies.Length; i++)
        {
            var         dependencyDesc  = serialisedDependencies[i];
            Type        keyType         = Type.GetType(dependencyDesc.typeName);

            TryAddDependencyInternal(keyType, dependencyDesc.dependency);
        }

        // set as initialised //
        initialised = true;
    }

    // *****************************
    // Get
    // *****************************
    public T Get<T>() where T : class
    {
        NotInitializedException();

        // check if no such key //
        bool keyFound = dependencies.TryGetValue(typeof(T), out LogicBase value);
        if (!keyFound)
        {
            throw new System.Exception($"No dependency of type '{typeof(T).ToString()}' found!");
        }

        T result = value as T;

        // check cast //
        bool castFailed = result == null;
        if (castFailed)
        {
            throw new System.Exception(string.Format($"Failed to cast type '{value.GetType()}' to iface '{typeof(T)}'"));
        }

        return result;
    }

    // *****************************
    // AddDependency
    // *****************************
    public void TryAddDependency(Type _typeKey, LogicBase _obj)
    {
        NotInitializedException();
        TryAddDependencyInternal(_typeKey, _obj);
    }

    // *****************************
    // TryAddDependencyInternal
    // *****************************
    void TryAddDependencyInternal(Type _typeKey, LogicBase _obj)
    {
        //var     type                            = _keyType == null ? null : _keyType.GetType();
        bool    implementsDependencyIface       = (_obj as IDependency) != null;
        bool    typeIsOk                        = implementsDependencyIface && CheckIfTypeIsValid(_typeKey);
        if (!typeIsOk)
        {
            throw new Exception($" Cannot find type OR type is invalid: {_typeKey}");
        }

        dependencies.Add(_typeKey, _obj);
    }

    // *****************************
    // CheckIfTypeIsValid
    // *****************************
    bool CheckIfTypeIsValid(Type _keyType)
    {
        bool isNull = _keyType == null;
        if (isNull)
        {
            return false;
        }

        var     typeInfo        = _keyType.GetTypeInfo();
        bool    classOrIface    = typeInfo.IsClass || typeInfo.IsInterface;
        if (classOrIface)
        {
            return true;
        }

        return false;
    }


    // *****************************
    // NotInitializedException
    // *****************************
    void NotInitializedException()
    {
        if (!initialised)
        {
            throw new System.Exception($"DependencyContainer {this} MUST be initialised before ussage!");
        }
    }

}