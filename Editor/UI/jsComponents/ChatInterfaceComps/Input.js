import React from "react"
import { cn } from "@/lib/utils"

const Input = React.forwardRef(({ className, ...props }, ref) => {
  return (
    <input
      className={cn(
        "flex h-9 w-full rounded-md border bg-transparent px-3 py-1",
        "text-sm shadow-sm transition-colors",
        "placeholder:text-gray-500",
        "focus:outline-none focus:ring-1 focus:ring-gray-700",
        "disabled:cursor-not-allowed disabled:opacity-50",
        "border-gray-800",
        className
      )}
      ref={ref}
      {...props}
    />
  )
})
Input.displayName = "Input"

export { Input }