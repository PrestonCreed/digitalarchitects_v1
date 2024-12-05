'use client';

import React, { useState, useRef, useEffect } from 'react';
import { X, Send, Plus, Maximize2, Minimize2, MessageSquare, Bot, User } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { ScrollArea } from '@/components/ui/scroll-area';
import { Separator } from '@/components/ui/separator';

const architects = [
  {
    id: 1,
    name: 'Environment Architect',
    status: 'active',
    model: 'GPT-4',
    lastActive: 'Now',
    description: 'Specialized in world building and environment design',
  },
  {
    id: 2,
    name: 'Character Architect',
    status: 'idle',
    model: 'Claude-2',
    lastActive: '5m ago',
    description: 'Focused on character development and dialogue',
  },
  {
    id: 3,
    name: 'System Architect',
    status: 'offline',
    model: 'GPT-4',
    lastActive: '1h ago',
    description: 'Handles game mechanics and systems design',
  },
];

export default function DraggablePopupChat() {
  const [messages, setMessages] = useState([]);
  const [inputMessage, setInputMessage] = useState('');
  const [currentArchitect, setCurrentArchitect] = useState(architects[0]);
  const [position, setPosition] = useState({ x: 20, y: 20 });
  const [isDragging, setIsDragging] = useState(false);
  const [isResizing, setIsResizing] = useState(false);
  const [size, setSize] = useState({ width: 400, height: 600 });
  const [isMaximized, setIsMaximized] = useState(false);
  const [showArchitectList, setShowArchitectList] = useState(false);

  const dragRef = useRef(null);
  const chatRef = useRef(null);

  useEffect(() => {
    const handleMouseMove = (e) => {
      if (isDragging) {
        setPosition((prev) => ({
          x: prev.x + e.movementX,
          y: prev.y + e.movementY,
        }));
      } else if (isResizing) {
        setSize((prev) => ({
          width: Math.max(400, prev.width + e.movementX),
          height: Math.max(400, prev.height + e.movementY),
        }));
      }
    };

    const handleMouseUp = () => {
      setIsDragging(false);
      setIsResizing(false);
    };

    if (isDragging || isResizing) {
      document.addEventListener('mousemove', handleMouseMove);
      document.addEventListener('mouseup', handleMouseUp);
    }

    return () => {
      document.removeEventListener('mousemove', handleMouseMove);
      document.removeEventListener('mouseup', handleMouseUp);
    };
  }, [isDragging, isResizing]);

  const handleMouseDown = (e) => {
    if (e.target === dragRef.current) {
      setIsDragging(true);
    }
  };

  const handleResizeMouseDown = (e) => {
    e.preventDefault();
    setIsResizing(true);
  };

  const handleSendMessage = () => {
    if (inputMessage.trim()) {
      const newMessage = {
        id: Date.now(),
        text: inputMessage,
        sender: 'user',
        timestamp: new Date().toLocaleTimeString(),
      };
      setMessages([...messages, newMessage]);
      setInputMessage('');

      setTimeout(() => {
        const architectResponse = {
          id: Date.now(),
          text: `Response from ${currentArchitect.name}`,
          sender: 'architect',
          timestamp: new Date().toLocaleTimeString(),
        };
        setMessages((prevMessages) => [...prevMessages, architectResponse]);
      }, 1000);
    }
  };

  const toggleMaximize = () => {
    setIsMaximized(!isMaximized);
    if (!isMaximized) {
      setSize({ width: window.innerWidth - 40, height: window.innerHeight - 40 });
      setPosition({ x: 20, y: 20 });
    } else {
      setSize({ width: 400, height: 600 });
      setPosition({ x: 20, y: 20 });
    }
  };

  return (
    <div
      className="fixed bg-black border border-gray-800 rounded-lg shadow-2xl overflow-hidden flex flex-col"
      style={{
        left: `${position.x}px`,
        top: `${position.y}px`,
        width: `${size.width}px`,
        height: `${size.height}px`,
      }}
    >
      {/* Header */}
      <div
        ref={dragRef}
        onMouseDown={handleMouseDown}
        className="h-10 bg-gray-900 border-b border-gray-800 flex items-center justify-between px-4 cursor-move select-none"
      >
        <div className="flex items-center gap-2">
          <MessageSquare size={16} className="text-gray-400" />
          <span className="text-sm font-medium text-gray-200">Chat with Architect</span>
        </div>
        <div className="flex items-center gap-2">
          <Button
            variant="ghost"
            size="sm"
            className="h-6 w-6 p-0 hover:bg-gray-800"
            onClick={toggleMaximize}
          >
            {isMaximized ? <Minimize2 size={14} /> : <Maximize2 size={14} />}
          </Button>
          <Button variant="ghost" size="sm" className="h-6 w-6 p-0 hover:bg-gray-800">
            <X size={14} />
          </Button>
        </div>
      </div>

      {/* Main Content */}
      <div className="flex-1 flex overflow-hidden">
        {/* Architect List */}
        <div
          className={`w-64 bg-gray-900 border-r border-gray-800 flex-shrink-0 ${
            showArchitectList ? '' : 'hidden'
          }`}
        >
          <ScrollArea className="h-full">
            <div className="p-4 space-y-4">
              <h3 className="text-sm font-medium text-gray-400 px-2">Architects</h3>
              {architects.map((architect) => (
                <button
                  key={architect.id}
                  onClick={() => setCurrentArchitect(architect)}
                  className={`w-full p-3 rounded-lg text-left transition-colors ${
                    currentArchitect.id === architect.id
                      ? 'bg-gray-800 text-white'
                      : 'text-gray-400 hover:bg-gray-800/50'
                  }`}
                >
                  <div className="flex items-center justify-between">
                    <span className="font-medium">{architect.name}</span>
                    <span
                      className={`inline-block w-2 h-2 rounded-full ${
                        architect.status === 'active'
                          ? 'bg-green-400'
                          : architect.status === 'idle'
                          ? 'bg-yellow-400'
                          : 'bg-gray-400'
                      }`}
                    />
                  </div>
                  <div className="text-xs mt-1 text-gray-500">{architect.model}</div>
                  <div className="text-xs mt-1 text-gray-500">{architect.description}</div>
                  <div className="text-xs mt-1 text-gray-600">Last active: {architect.lastActive}</div>
                </button>
              ))}
            </div>
          </ScrollArea>
        </div>

        {/* Chat Area */}
        <div className="flex-1 flex flex-col bg-gray-900">
          <ScrollArea className="flex-1 p-4">
            <div className="space-y-4" ref={chatRef}>
              {messages.map((message) => (
                <div
                  key={message.id}
                  className={`flex items-start gap-3 ${
                    message.sender === 'user' ? 'justify-end' : 'justify-start'
                  }`}
                >
                  {message.sender === 'architect' && <Bot size={20} className="mt-1 text-gray-500" />}
                  <div
                    className={`p-3 rounded-lg max-w-[70%] ${
                      message.sender === 'user'
                        ? 'bg-blue-600 text-white'
                        : 'bg-gray-800 text-gray-200'
                    }`}
                  >
                    <div className="text-sm">{message.text}</div>
                    <div className="text-xs mt-1 opacity-60">{message.timestamp}</div>
                  </div>
                  {message.sender === 'user' && <User size={20} className="mt-1 text-gray-500" />}
                </div>
              ))}
            </div>
          </ScrollArea>

          {/* Input Area */}
          <div className="p-4 border-t border-gray-800 bg-gray-900">
            <div className="flex items-center gap-2">
              <Button
                variant="ghost"
                size="sm"
                className="h-10 px-3 text-gray-400 hover:text-gray-300"
                onClick={() => setShowArchitectList(!showArchitectList)}
              >
                <Bot size={20} />
              </Button>
              <Input
                type="text"
                value={inputMessage}
                onChange={(e) => setInputMessage(e.target.value)}
                onKeyPress={(e) => e.key === 'Enter' && handleSendMessage()}
                placeholder="Type your message..."
                className="flex-1 bg-gray-800 border-gray-700 text-gray-200 placeholder-gray-500"
              />
              <Button
                onClick={handleSendMessage}
                size="sm"
                className="h-10 px-3 bg-blue-600 hover:bg-blue-700"
              >
                <Send size={16} />
              </Button>
              <Button
                size="sm"
                variant="ghost"
                className="h-10 px-3 text-gray-400 hover:text-gray-300"
              >
                <Plus size={20} />
              </Button>
            </div>
          </div>
        </div>
      </div>

      {/* Resize Handle */}
      <div
        className="absolute bottom-0 right-0 w-4 h-4 cursor-se-resize"
        onMouseDown={handleResizeMouseDown}
      />
    </div>
  );
}